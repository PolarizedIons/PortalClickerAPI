using System;
using PortalClickerApi.Database.Models;

namespace PortalClickerApi.Models.Responses
{
    public class PlayerResponse
    {
        private readonly ClickerPlayer _player;

        public PlayerResponse(ClickerPlayer player)
        {
            _player = player;
        }

        public Guid UserId => _player.UserId;
        public ulong PortalCount => _player.PortalCount;
        public ulong PortalsPerSecond => _player.PortalsPerSecond;
        public ulong BaseClickAmount => _player.BaseClickAmount;
        public double ClickMultiplier => _player.ClickMultiplier;
        public double ItemPriceMultiplier => _player.ItemPriceMultiplier;
        public double ItemPortalMultiplier => _player.ItemPortalMultiplier;
    }
}
