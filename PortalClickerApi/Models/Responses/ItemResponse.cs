using System;
using PortalClickerApi.Database.Models;
using PortalClickerApi.Extentions;

namespace PortalClickerApi.Models.Responses
{
    public class ItemResponse
    {
        private readonly ClickerPlayer _player;
        private readonly ClickerSystemItem _systemItem;
        private readonly ClickerUserItem _userItem;

        public ItemResponse(ClickerPlayer player, ClickerSystemItem systemItem, ClickerUserItem userItem = null)
        {
            _player = player;
            _systemItem = systemItem;
            _userItem = userItem;
        }

        public Guid Id => _systemItem.Id;
        public string Name => _systemItem.Name;
        public string Description => _systemItem.Description;
        public ulong Amount => _userItem?.Amount ?? 0;
        public ulong Cost => (ulong)((_userItem?.NextCost ?? ClickerSystemItem.CalculateCost(_systemItem.CostExpression, 0)) * _player.ItemPriceMultiplier);
        public double Portals => (_systemItem.Portals * _player.ItemPortalMultiplier).FloorTo(2);
    }
}
