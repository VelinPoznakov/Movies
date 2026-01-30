using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Movie.Dtos.Auth;
using Movie.Entities;
using Movie.Services.Interfaces;

namespace Movie.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthService(UserManager<AppUser> userManager, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<AuthTokens> RegisterUser(RegisterDto request)
        {
            var user = new AppUser
            {
                UserName = request.Username,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                throw new InvalidOperationException("User registration failed.");

            var accessToken = GenerateTokenString(user.UserName!, user.Id);
            var refreshToken = GenerateRefreshToken();
            var refreshExpires = DateTime.UtcNow.AddHours(12);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshExpires;
            user.LastLogInAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return new AuthTokens
            {
                IsLoggedIn = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = refreshExpires
            };
        }

        public async Task<AuthTokens> LoginUser(LoginUserDto request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var ok = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!ok)
                throw new InvalidOperationException("Invalid password.");

            var accessToken = GenerateTokenString(user.UserName!, user.Id);
            var refreshToken = GenerateRefreshToken();
            var refreshExpires = DateTime.UtcNow.AddHours(12);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshExpires;
            user.LastLogInAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return new AuthTokens
            {
                IsLoggedIn = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = refreshExpires
            };
        }

        public async Task<AuthTokens> RefreshToken(string refreshToken)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user == null)
                throw new InvalidOperationException("Invalid refresh token.");

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new InvalidOperationException("Refresh token expired.");

            var newAccessToken = GenerateTokenString(user.UserName!, user.Id);
            var newRefreshToken = GenerateRefreshToken();
            var newRefreshExpires = DateTime.UtcNow.AddHours(12);

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = newRefreshExpires;
            user.LastLogInAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return new AuthTokens
            {
                IsLoggedIn = true,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiryTime = newRefreshExpires
            };
        }

        public async Task LogoutUserAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) throw new InvalidOperationException("User not found.");

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        public async Task<ProfileResponseDto> GetProfileAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) throw new InvalidOperationException("User not found.");

            return _mapper.Map<ProfileResponseDto>(user);
        }

        private string GenerateTokenString(string username, string userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, "User"),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }
    }
}
