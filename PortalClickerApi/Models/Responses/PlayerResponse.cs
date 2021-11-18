using System;
using PortalClickerApi.Database.Models;
using PortalClickerApi.Extentions;

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
        public uint PortalCount => (uint)_player.PortalCount;
        public double PortalsPerSecond => _player.PortalsPerSecond.FloorTo(1);
        public ulong BaseClickAmount => _player.BaseClickAmount;
        public double ClickMultiplier => _player.ClickMultiplier;
        public double ItemPriceMultiplier => _player.ItemPriceMultiplier;
        public double ItemPortalMultiplier => _player.ItemPortalMultiplier;
    }
}
