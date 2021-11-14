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
        private const int MaxDelta = 30;
        private const int ClicksPerSecond = 10;

        private readonly DatabaseContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<ClickerHub, IClickerHubClient> _clickerHub;

        public ClickerService(DatabaseContext db, UserManager<ApplicationUser> userManager, IHubContext<ClickerHub, IClickerHubClient> clickerHub)
        {
            _db = db;
            _userManager = userManager;
            _clickerHub = clickerHub;
        }

        public async Task<ulong> Tick(Guid userId, int delta, string connectionId)
        {
            var player = await GetPlayer(userId);
            if (!ValidateTick(player, delta))
            {
                return player.PortalCount;
            }

            var addition = (ulong)delta * player.PortalsPerSecond;
            player.PortalCount += addition;
            player.LastTick = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            await _clickerHub.Clients.GroupExcept(userId.ToString(), connectionId).OnPortalCountUpdated(player.PortalCount);

            return player.PortalCount;
        }

        public async Task<ulong> Click(Guid userId, string connectionId)
        {
            var player = await GetPlayer(userId);
            if (!ValidateClick(player))
            {
                return player.PortalCount;
            }

            player.PortalCount += player.BaseClickAmount;
            player.LastClick = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            await _clickerHub.Clients.GroupExcept(userId.ToString(), connectionId).OnPortalCountUpdated(player.PortalCount);

            return player.PortalCount;
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

            await _clickerHub.Clients.Group(userId.ToString()).OnUpgradePurchased(upgrade.Id);
            
            return new UpgradeResponse(upgrade, true);
        }

        private bool ValidateTick(ClickerPlayer player, int delta)
        {
            if (delta > MaxDelta)
            {
                return false;
            }

            if (player.LastTick.HasValue)
            {
                return player.LastTick.Value.AddSeconds(delta) < DateTime.UtcNow;
            }

            return true;
        }

        private bool ValidateClick(ClickerPlayer player)
        {
            if (player.LastClick.HasValue)
            {
                return player.LastClick.Value.AddSeconds((double)1 / ClicksPerSecond) < DateTime.UtcNow;
            }

            return true;
        }

        private async Task<ClickerPlayer> GetPlayer(Guid userId)
        {
            var player = await _db.ClickerPlayers.FirstOrDefaultAsync(x => x.User.Id == userId) 
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
    }
}
