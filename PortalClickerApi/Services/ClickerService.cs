using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PortalClickerApi.Database;
using PortalClickerApi.Database.Models;
using PortalClickerApi.Extentions;
using PortalClickerApi.Identity;

namespace PortalClickerApi.Services
{
    public class ClickerService : IScopedDiService
    {
        private const int MaxDelta = 30;
        private const int ClicksPerSecond = 10;

        private readonly DatabaseContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClickerService(DatabaseContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<ulong> Tick(Guid userId, int delta)
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

            return player.PortalCount;
        }

        public async Task<ulong> Click(Guid userId)
        {
            var player = await GetPlayer(userId);
            if (!ValidateClick(player))
            {
                return player.PortalCount;
            }

            player.PortalCount += 1;
            player.LastClick = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return player.PortalCount;
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
    }
}
