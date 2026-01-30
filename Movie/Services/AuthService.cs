using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            if(!result.Succeeded) 
                throw new InvalidOperationException("User registration failed.");
                
            var token = GenerateTockenString(user.UserName!, user.Id);
            var refreshToken = GenerateRefreshToken();
            var refreshExpiresDate = DateTime.UtcNow.AddHours(12);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshExpiresDate;
            user.LastLogInAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

<<<<<<< Updated upstream
            return new TokenDto{ Token = token, RefreshToken = refreshToken, IsLoggedIn = true };
=======
            return new AuthTokens{AccessToken = token, RefreshToken = refreshToken, RefreshTokenExpiryTime = refreshExpiresDate};
>>>>>>> Stashed changes
        }

        public async Task<AuthTokens> LoginUser(LoginUserDto request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            
            if(user == null) 
                throw new InvalidOperationException("User not found.");

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if(!result) 
                throw new InvalidOperationException("Invalid password.");

            var token = GenerateTockenString(user.UserName!, user.Id);
            var refreshToken = GenerateRefreshToken();
            var refreshExpiresDate = DateTime.UtcNow.AddHours(12);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshExpiresDate;
            user.LastLogInAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

<<<<<<< Updated upstream
            return new TokenDto{ Token = token, RefreshToken = refreshToken, IsLoggedIn = true };
=======
            return new AuthTokens{AccessToken = token, RefreshToken = refreshToken, RefreshTokenExpiryTime = refreshExpiresDate};
        }

        public async Task LogoutUserAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if(user == null) 
                throw new InvalidOperationException("User not found.");

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
>>>>>>> Stashed changes
        }

        public async Task<ProfileResponseDto> GetProfileAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) throw new InvalidOperationException("User not found.");

            return _mapper.Map<ProfileResponseDto>(user);

        }

        public async Task<AuthTokens> RefreshToken(string refreshToken)
        {
<<<<<<< Updated upstream
            var principal = GetTokenPrincipal(request.Token);
            
            if(principal?.Identity?.Name is null) throw new InvalidOperationException("Invalid token.");

            var user = await _userManager.FindByNameAsync(principal.Identity.Name);

            if(user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
=======
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user == null)
>>>>>>> Stashed changes
                throw new InvalidOperationException("Invalid refresh token.");

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new InvalidOperationException("Refresh token expired.");

            var newAccessToken = GenerateTockenString(user.UserName!, user.Id);
            var newRefreshToken = GenerateRefreshToken();
            var newRefreshExpires = DateTime.UtcNow.AddHours(12);

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = newRefreshExpires;
            user.LastLogInAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

<<<<<<< Updated upstream
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
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]!));

            var validate = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _config["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["JWT:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey
=======
            return new AuthTokens
            {
                IsLoggedIn = true,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiryTime = newRefreshExpires
>>>>>>> Stashed changes
            };
        }

        private string GenerateTockenString(string username, string userId)
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
                claims:claims,
                expires: DateTime.UtcNow.AddSeconds(60),
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
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