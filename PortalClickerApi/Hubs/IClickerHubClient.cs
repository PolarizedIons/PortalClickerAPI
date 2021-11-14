using System;
using System.Threading.Tasks;

namespace PortalClickerApi.Hubs
{
    public interface IClickerHubClient
    {
        public Task OnUpgradePurchased(Guid id);
        public Task OnPortalCountUpdated(ulong portalCount);
    }
}
