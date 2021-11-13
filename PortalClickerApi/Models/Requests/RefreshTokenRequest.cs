using System;

namespace PortalClickerApi.Models.Requests
{
    public class RefreshTokenRequest
    {
        public Guid UserId { get; set; }
        public Guid RefreshToken { get; set; }
    }
}
