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
    }
}
