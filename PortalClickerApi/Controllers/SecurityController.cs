using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalClickerApi.Database;
using PortalClickerApi.Extentions;
using PortalClickerApi.Models.Requests;
using PortalClickerApi.Models.Responses;
using PortalClickerApi.Services;

namespace PortalClickerApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("security")]
    public class SecurityController : ControllerBase
    {
        private readonly SecurityService _securityService;

        public SecurityController(SecurityService securityService)
        {
            _securityService = securityService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest payload)
        {
            var result = await _securityService.Register(payload);
            return Ok(result);
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest payload)
        {
            var result = await _securityService.Login(payload);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<LoginRequest>> RefreshToken([FromBody] RefreshTokenRequest payload)
        {
            var result = await _securityService.Refresh(payload);
            return Ok(result);
        }
        
        [HttpGet("temp")]
        [NoTransaction]
        public async Task<ActionResult<string>> Temp()
        {
            return Ok($"Hello {this.GetUserName()}!");
        }
    }
}
