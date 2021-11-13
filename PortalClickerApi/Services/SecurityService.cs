using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PortalClickerApi.Config;
using PortalClickerApi.Database;
using PortalClickerApi.Exceptions;
using PortalClickerApi.Extentions;
using PortalClickerApi.Identity;
using PortalClickerApi.Models.Requests;
using PortalClickerApi.Models.Responses;

namespace PortalClickerApi.Services
{
    public class SecurityService : IScopedDiService
    {
        private static readonly TimeSpan AccessTokenValidFor = TimeSpan.FromHours(6);
        private static readonly TimeSpan RefreshTokenValidFor = TimeSpan.FromDays(30);

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApiConfig _config;
        private readonly DatabaseContext _db;
        private readonly SigningCredentials _jwtKey;

        public SecurityService(UserManager<ApplicationUser> userManager, ApiConfig config, DatabaseContext db)
        {
            _userManager = userManager;
            _config = config;
            _db = db;


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Jwt.Secret));
            _jwtKey = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        public async Task<LoginResponse> Register(RegisterRequest payload)
        {
            var user = await CreateUser(payload.Email, payload.UserName, payload.Password);
            var (accessToken, refreshToken) = await CreateTokens(user);
            return new LoginResponse(user, accessToken, refreshToken);
        }

        public async Task<LoginResponse> Login(LoginRequest payload)
        {
            var user = await LoginUser(payload.UserName, payload.Password);
            var (accessToken, refreshToken) = await CreateTokens(user);
            return new LoginResponse(user, accessToken, refreshToken);
        }

        public async Task<object> Refresh(RefreshTokenRequest payload)
        {
            var user = await GetUser(payload.UserId);
            await ExpireRefreshToken(user, payload.RefreshToken);
            var (accessToken, refreshToken) = await CreateTokens(user);
            return new LoginResponse(user, accessToken, refreshToken);
        }

        private async Task<ApplicationUser> CreateUser(string email, string username, string password)
        {
            var user = new ApplicationUser
            {
                Email = email,
                UserName = username,
            };
            await _userManager.CreateAsync(user, password)
                .ThrowIfNotSucceeded("Unable to create user");

            return user;
        }

        private async Task<ApplicationUser> LoginUser(string userName, string password)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                throw new BadRequestException("Invalid username/password");
            }

            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                throw new BadRequestException("Invalid username/password");
            }

            return user;
        }

        private async Task<(string accessToken, RefreshToken refreshToken)> CreateTokens(ApplicationUser user)
        {
            var accessToken = await CreateAccessToken(user);
            var refreshToken = await CreateRefreshToken(user);
            return (accessToken, refreshToken);
        }

        private async Task<string> CreateAccessToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
            claims.AddRange(roleClaims);

            var expires = DateTime.UtcNow.Add(AccessTokenValidFor);

            var token = new JwtSecurityToken(
                issuer: _config.Jwt.Issuer,
                audience: _config.Jwt.Audience,
                claims,
                expires : expires,
                signingCredentials : _jwtKey
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<RefreshToken> CreateRefreshToken(ApplicationUser user)
        {
            var token = new RefreshToken
            {
                User = user,
                ExpiresUtc = DateTime.UtcNow.Add(RefreshTokenValidFor),
            };

            await _db.AddAsync(token);
            user.RefreshTokens.Add(token);

            return token;
        }

        private async Task<ApplicationUser> GetUser(Guid userId)
        {
            var user = await _userManager.Users
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new BadRequestException("User not found");
            }

            return user;
        }

        private Task ExpireRefreshToken(ApplicationUser user, Guid tokenId)
        {
            var token = user.RefreshTokens.FirstOrDefault(x => x.Id == tokenId);
            if (token == null)
            {
                throw new BadRequestException("Invalid token");
            }

            _db.Remove(token);
            user.RefreshTokens.Remove(token);

            return Task.CompletedTask;
        }
    }
}
