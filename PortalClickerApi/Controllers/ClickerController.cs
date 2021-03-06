using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalClickerApi.Database;
using PortalClickerApi.Extentions;
using PortalClickerApi.Models.Responses;
using PortalClickerApi.Services;

namespace PortalClickerApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("clicker")]
    public class ClickerController : ControllerBase
    {
        private readonly ClickerService _clickerService;

        public ClickerController(ClickerService clickerService)
        {
            _clickerService = clickerService;
        }

        [NoTransaction]
        [HttpGet("stats")]
        public async Task<ActionResult<PlayerResponse>> GetSelfStats()
        {
            var result = await _clickerService.GetStats(this.GetUserId());
            return Ok(result);
        }

        [NoTransaction]
        [HttpGet("upgrade")]
        public async Task<ActionResult<IEnumerable<UpgradeResponse>>> GetUpgrades()
        {
            var result = await _clickerService.GetUpgrades(this.GetUserId());
            return Ok(result);
        }

        [HttpPost("upgrade/{id:guid}")]
        public async Task<ActionResult<UpgradeResponse>> PurchaseUpgrade([FromRoute] Guid id)
        {
            var result = await _clickerService.PurchaseUpgrade(this.GetUserId(), id);
            return Ok(result);
        }

        [NoTransaction]
        [HttpGet("item")]
        public async Task<ActionResult<IEnumerable<ItemResponse>>> GetItems()
        {
            var result = await _clickerService.GetItems(this.GetUserId());
            return Ok(result);
        }

        [HttpPost("item/{id:guid}")]
        public async Task<ActionResult<IEnumerable<ItemResponse>>> PurcahseItem(Guid id)
        {
            var result = await _clickerService.PurchaseItem(this.GetUserId(), id);
            return Ok(result);
        }

        [NoTransaction]
        [HttpGet("leaderboard")]
        public async Task<ActionResult<IEnumerable<LeaderboardResponse>>> GetLeaderboard()
        {
            var result = await _clickerService.GetLeaderboard();
            return Ok(result);
        }
    }
}
