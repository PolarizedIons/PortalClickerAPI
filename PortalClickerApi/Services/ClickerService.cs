using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PortalClickerApi.Database;
using PortalClickerApi.Database.Models;
using PortalClickerApi.Exceptions;
using PortalClickerApi.Extentions;
using PortalClickerApi.Hubs;
using PortalClickerApi.Identity;
using PortalClickerApi.Models;
using PortalClickerApi.Models.Responses;

namespace PortalClickerApi.Services
{
    public class ClickerService : IScopedDiService
    {
        private const int TickMaxDelta = 30; // max 30 seconds between ticks
        private const int ClicksPerSecond = 20; // max 20 clicks per second
        private const double ClickMinDelta = 1d / ClicksPerSecond;  // min amount of time to pass per click
        private const int ClickMaxCount = ClicksPerSecond * 5; // max amount the user can click per event

        private readonly DatabaseContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<ClickerHub, IClickerHubClient> _clickerHub;

        public ClickerService(DatabaseContext db, UserManager<ApplicationUser> userManager, IHubContext<ClickerHub, IClickerHubClient> clickerHub)
        {
            _db = db;
            _userManager = userManager;
            _clickerHub = clickerHub;
        }

        public async Task<uint> Tick(Guid userId, double delta, string connectionId = null)
        {
            var player = await GetPlayer(userId);
            if (!ValidateTick(player, delta))
            {
                return (uint)player.PortalCount;
            }

            var addition = (ulong)(delta * player.PortalsPerSecond);
            player.PortalCount += addition;
            player.LastTick = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            if (string.IsNullOrEmpty(connectionId))
            {
                await _clickerHub.Clients.Group(userId.ToString()).OnPortalCountUpdated((uint)player.PortalCount);
            }
            else
            {
                await _clickerHub.Clients.GroupExcept(userId.ToString(), connectionId).OnPortalCountUpdated((uint)player.PortalCount);
            }

            return (uint)player.PortalCount;
        }

        public async Task<uint> Click(Guid userId, ulong amount, string connectionId)
        {
            var player = await GetPlayer(userId);
            if (!ValidateClick(player, amount))
            {
                return (uint)player.PortalCount;
            }

            player.PortalCount += player.BaseClickAmount * amount;
            player.LastClick = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            await _clickerHub.Clients.GroupExcept(userId.ToString(), connectionId).OnPortalCountUpdated((uint)player.PortalCount);

            return (uint)player.PortalCount;
        }

        public async Task<IEnumerable<UpgradeResponse>> GetUpgrades(Guid userId)
        {
            return await _db.Upgrades
                    .Select(x => new { upgrade = x, purchased = x.Players.Any(p => p.UserId == userId) })
                    .OrderBy(x => x.upgrade.Price)
                    .Select(x => new UpgradeResponse(x.upgrade, x.purchased))
                    .ToListAsync();
        }

        public async Task<UpgradeResponse> PurchaseUpgrade(Guid userId, Guid upgradeId)
        {
            var upgrade = _db.Upgrades.FirstOrDefault(x => x.Id == upgradeId);
            if (upgrade == null)
            {
                throw new BadRequestException("No such upgrade");
            }

            var player = _db.ClickerPlayers
                .Include(x => x.Upgrades)
                .First(x => x.UserId == userId);

            if (player.PortalCount < upgrade.Price)
            {
                throw new BadRequestException("Not enough Portals");
            }

            if (player.Upgrades.Contains(upgrade))
            {
                throw new BadRequestException("You already have that upgrade!");
            }

            player.PortalCount -= upgrade.Price;
            player.Upgrades.Add(upgrade);
            ApplyUpgradeStats(player, upgrade);
            await _db.SaveChangesAsync();

            var response = new UpgradeResponse(upgrade, true);
            await _clickerHub.Clients.Group(userId.ToString()).OnUpgradePurchased(response);
            await _clickerHub.Clients.Group(userId.ToString()).OnPlayerStatsUpdated(new PlayerResponse(player));
            return response;
        }

        public async Task<IEnumerable<ItemResponse>> GetItems(Guid userId)
        {
            var player = await _db.ClickerPlayers.FirstAsync(x => x.UserId == userId);
            var systemItems = await _db.SystemItems
                .Where(x => x.UserItems.All(ui => ui.User.Id != userId))
                .Select(x => new ItemDbResult { SystemItem = x, UserItem = null })
                .ToListAsync();
            var userItems = await _db.UserItems
                .Where(x => x.User.Id == userId)
                .Select(x => new ItemDbResult { UserItem = x, SystemItem = x.SystemItem })
                .ToListAsync();

            return systemItems
                .Union(userItems)
                .OrderBy(x => ClickerSystemItem.CalculateCost(x.SystemItem.CostExpression, 0))
                .Select(x => new ItemResponse(player, x.SystemItem, x.UserItem));
        }

        public async Task<ItemResponse> PurchaseItem(Guid userId, Guid id)
        {
            var player = await GetPlayer(userId);
            var userItem = await GetUserItemFromSystemId(player, id);
            var cost = userItem.NextCost * player.ItemPriceMultiplier;
            if (player.PortalCount < cost)
            {
                throw new BadRequestException("You cannot afford this item.");
            }

            await Tick(userId, (DateTime.UtcNow - (player.LastTick ?? DateTime.UtcNow)).TotalSeconds);

            player.PortalCount -= cost;
            player.PortalsPerSecond += userItem.SystemItem.Portals * player.ItemPortalMultiplier;
            userItem.Amount += 1;

            await _db.SaveChangesAsync();

            var response = new ItemResponse(player, userItem.SystemItem, userItem);
            await _clickerHub.Clients.Group(userId.ToString()).OnItemPurchased(response);
            return response;
        }

        public async Task<PlayerResponse> GetStats(Guid userId)
        {
            var player = await _db.ClickerPlayers.FirstAsync(x => x.UserId == userId);
            return new PlayerResponse(player);
        }

        private bool ValidateTick(ClickerPlayer player, double delta)
        {
            if (delta > TickMaxDelta || delta <= 0)
            {
                return false;
            }

            if (player.LastTick.HasValue)
            {
                var diff = Math.Abs((DateTime.UtcNow - player.LastTick.Value.AddSeconds(delta)).Milliseconds);
                return diff <= 1000; // allow 1000ms of error 
            }

            return true;
        }

        private bool ValidateClick(ClickerPlayer player, ulong amount)
        {
            if (ClickMaxCount < amount)
            {
                return false;
            }

            if (player.LastClick.HasValue)
            {
                return player.LastClick.Value.AddSeconds(ClickMinDelta * amount) < DateTime.UtcNow;
            }

            return true;
        }

        private async Task<ClickerPlayer> GetPlayer(Guid userId)
        {
            var player = await _db.ClickerPlayers
                             .Include(x => x.User)
                             .FirstOrDefaultAsync(x => x.User.Id == userId) 
                         ?? await CreatePlayer(userId);
            return player;
        }

        private async Task<ClickerPlayer> CreatePlayer(Guid userId)
        {
            var user = _userManager.Users.First(x => x.Id == userId);

            var player = new ClickerPlayer
            {
                Items = new List<ClickerUserItem>(),
                Upgrades = new List<ClickerUpgrade>(),
                UserId = userId,
                User = user,
                LastTick = DateTime.UtcNow,
                PortalCount = 0,
                PortalsPerSecond = 0,
                BaseClickAmount = 1,
                ClickMultiplier = 1.0,
                ItemPortalMultiplier = 1.0,
                ItemPriceMultiplier = 1.0
            };

            await _db.ClickerPlayers.AddAsync(player);
            return player;
        }

        private void ApplyUpgradeStats(ClickerPlayer player, ClickerUpgrade upgrade)
        {
            if ((upgrade.MultiplierType & UpgradeMultiplierType.AddClick) == UpgradeMultiplierType.AddClick)
            {
                player.BaseClickAmount += (ulong)upgrade.MultiplierAmount;
            }

            if ((upgrade.MultiplierType & UpgradeMultiplierType.Click) == UpgradeMultiplierType.Click)
            {
                player.ClickMultiplier *= upgrade.MultiplierAmount;
            }
            
            if ((upgrade.MultiplierType & UpgradeMultiplierType.ItemPortals) == UpgradeMultiplierType.ItemPortals)
            {
                player.ItemPortalMultiplier *= upgrade.MultiplierAmount;
            }
            
            if ((upgrade.MultiplierType & UpgradeMultiplierType.ItemPrice) == UpgradeMultiplierType.ItemPrice)
            {
                player.ItemPriceMultiplier *= upgrade.MultiplierAmount;
            }
        }

        private async Task<ClickerUserItem> GetUserItemFromSystemId(ClickerPlayer player, Guid systemItemId)
        {
            var item = await _db.UserItems
                .Include(x => x.SystemItem)
                .FirstOrDefaultAsync(x => x.SystemItem.Id == systemItemId && x.User.Id == player.UserId);

            if (item == null)
            {
                var systemItem = _db.SystemItems.First(x => x.Id == systemItemId);

                item = new ClickerUserItem
                {
                    Amount = 0,
                    Player = player,
                    User = player.User,
                    SystemItem = systemItem, 
                };
                _db.UserItems.Add(item);
            }

            return item;
        }
    }

    internal class ItemDbResult
    {
        public ClickerSystemItem SystemItem { get; set; }
        public ClickerUserItem UserItem { get; set; }
    }
}
