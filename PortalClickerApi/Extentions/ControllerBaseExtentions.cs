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
            return controllerBase.User.GetUserId();
        }
        
        public static string GetUserName(this ControllerBase controllerBase)
        {
            return controllerBase.User.GetUserName();
        }
    }
}
