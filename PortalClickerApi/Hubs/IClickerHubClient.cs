using System;
using System.Threading.Tasks;
using PortalClickerApi.Models.Responses;

namespace PortalClickerApi.Hubs
{
    public interface IClickerHubClient
    {
        public Task OnUpgradePurchased(UpgradeResponse upgrade);
        public Task OnPortalCountUpdated(uint portalCount);
        public Task OnPlayerStatsUpdated(PlayerResponse player);
        public Task OnItemPurchased(ItemResponse item);
    }
}
