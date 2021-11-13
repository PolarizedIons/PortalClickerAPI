using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PortalClickerApi.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
