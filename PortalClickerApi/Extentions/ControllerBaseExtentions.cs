using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace PortalClickerApi.Extentions
{
    public static class ControllerBaseExtentions
    {
        public static Guid GetUserId(this ControllerBase controllerBase)
        {
            var claimsPrincipal = controllerBase.User;
            return Guid.Parse(claimsPrincipal.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value);
        }
        
        public static string GetUserName(this ControllerBase controllerBase)
        {
            var claimsPrincipal = controllerBase.User;
            return claimsPrincipal.Claims.First(x => x.Type == ClaimTypes.Name).Value;
        }
    }
}
