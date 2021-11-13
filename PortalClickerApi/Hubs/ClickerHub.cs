using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PortalClickerApi.Extentions;
using PortalClickerApi.Services;

namespace PortalClickerApi.Hubs
{
    [Authorize]
    public class ClickerHub : Hub
    {
        private readonly ClickerService _clickerService;

        public ClickerHub(ClickerService clickerService)
        {
            _clickerService = clickerService;
        }

        public async Task<ulong> Tick(int delta)
        {
            var userId = Context.User.GetUserId();
            var result = await _clickerService.Tick(userId, delta);
            return result;
        }

        public async Task<ulong> Click()
        {
            var userId = Context.User.GetUserId();
            var result = await _clickerService.Click(userId);
            return result;
        }
    }
}
