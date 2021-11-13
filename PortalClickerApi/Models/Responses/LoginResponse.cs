using System;
using PortalClickerApi.Identity;

namespace PortalClickerApi.Models.Responses
{
    public class LoginResponse
    {
        private readonly ApplicationUser _user;
        private readonly string _accessToken;
        private readonly RefreshToken _refreshToken;

        public LoginResponse(ApplicationUser user, string accessToken, RefreshToken refreshToken)
        {
            _user = user;
            _accessToken = accessToken;
            _refreshToken = refreshToken;
        }

        public Guid UserId => _user.Id;
        public string UserName => _user.UserName;
        public string AccessToken => _accessToken;
        public Guid RefreshToken => _refreshToken.Id;
    }
}
