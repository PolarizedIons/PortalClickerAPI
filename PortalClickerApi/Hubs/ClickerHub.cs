using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PortalClickerApi.Extentions;
using PortalClickerApi.Services;

namespace PortalClickerApi.Hubs
{
    [Authorize]
    public class ClickerHub : Hub<IClickerHubClient>
    {
        private readonly ClickerService _clickerService;

        public ClickerHub(ClickerService clickerService)
        {
            _clickerService = clickerService;
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.GetUserId().ToString());
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.GetUserId().ToString());
            await base.OnDisconnectedAsync(exception);
        }

        public async Task<ulong> Tick(int delta)
        {
            var userId = Context.User.GetUserId();
            var result = await _clickerService.Tick(userId, delta, Context.ConnectionId);
            return result;
        }

        public async Task<ulong> Click(ulong amount)
        {
            var userId = Context.User.GetUserId();
            var result = await _clickerService.Click(userId, amount, Context.ConnectionId);
            return result;
        }
    }
}
