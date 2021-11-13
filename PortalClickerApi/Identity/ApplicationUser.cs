using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using PortalClickerApi.Database.Models;

namespace PortalClickerApi.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ClickerPlayer Player { get; set; }
    }
}
