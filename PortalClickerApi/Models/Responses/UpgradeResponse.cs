using System;
using PortalClickerApi.Database.Models;

namespace PortalClickerApi.Models.Responses
{
    public class UpgradeResponse
    {
        private readonly ClickerUpgrade _upgrade;
        private readonly bool _purchased;

        public UpgradeResponse(ClickerUpgrade upgrade, bool purchased)
        {
            _upgrade = upgrade;
            _purchased = purchased;
        }

        public Guid Id => _upgrade.Id;
        public string Name => _upgrade.Name;
        public string Description => _upgrade.Description;
        public string ActionText => _upgrade.ActionText;
        public ulong Price => _upgrade.Price;
        public bool Purchased => _purchased;
    }
}
