using System;
using PortalClickerApi.Database.Models;

namespace PortalClickerApi.Identity
{
    public class RefreshToken : DbEntity
    {
        public ApplicationUser User { get; set; }
        public DateTime ExpiresUtc { get; set; }
    }
}
