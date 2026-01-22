using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Movie.Dtos.Auth;
using Movie.Entities;
using Movie.Services.Interfaces;

namespace Movie.Services
{
    public class AuthService: IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _config;
        public AuthService(UserManager<AppUser> userManager, IConfiguration config)
        {
            _config = config;
            _userManager = userManager;
        }

        public async Task<TokenDto> RegisterUser(RegisterDto request)
        {
            var user = new AppUser
            {
              UserName = request.Username,
              Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if(!result.Succeeded) 
                throw new InvalidOperationException("User registration failed.");
                
            var token = GenerateTockenString(user.UserName!);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(12);
            user.LastLogInAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return new TokenDto{ Token = token, RefreshToken = refreshToken, IsLoggedIn = true };
        }

        public async Task<TokenDto> LoginUser(LoginUserDto request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            
            if(user == null) 
                throw new InvalidOperationException("User not found.");

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if(!result) 
                throw new InvalidOperationException("Invalid password.");

            var token = GenerateTockenString(user.UserName!);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(12);
            user.LastLogInAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return new TokenDto{ Token = token, RefreshToken = refreshToken, IsLoggedIn = true };
        }

        public async Task<TokenDto> RefreshToken(RefreshTokenDto request)
        {
            var principal = GetTokenPrincipal(request.Token);
            
            var response = new TokenDto();
            if(principal?.Identity?.Name is null) throw new InvalidOperationException("Invalid token.");

            var user = await _userManager.FindByNameAsync(principal.Identity.Name);

            if(user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new InvalidOperationException("Invalid refresh token.");

            var token = GenerateTockenString(user.UserName!);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(12);
            user.LastLogInAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return new TokenDto{ Token = token, RefreshToken = refreshToken, IsLoggedIn = true };
        }

        public async Task LogoutUserAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if(user == null) throw new InvalidOperationException("User not found.");

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        private ClaimsPrincipal? GetTokenPrincipal(string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SigningKey"]!));

            var validate = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _config["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["JWT:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey
            };

            return new JwtSecurityTokenHandler().ValidateToken(token, validate, out _);
        }

        private string GenerateTockenString(string username)
        {
            var claims = new List<Claim>
            {
              new Claim(ClaimTypes.Name, username),
              new Claim(ClaimTypes.Role, "User"),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SigningKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims:claims,
                expires: DateTime.UtcNow.AddSeconds(60),
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                signingCredentials: credentials
                );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using (var numberGenerator = RandomNumberGenerator.Create())
            {
                numberGenerator.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);

        }
  }
}