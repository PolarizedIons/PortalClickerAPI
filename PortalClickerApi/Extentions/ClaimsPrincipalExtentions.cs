using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace PortalClickerApi.Extentions
{
    public static class ClaimsPrincipalExtentions
    {
        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            return Guid.Parse(claimsPrincipal.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value);
        }

        public static string GetUserName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.First(x => x.Type == ClaimTypes.Name).Value;
        }
    }
}
