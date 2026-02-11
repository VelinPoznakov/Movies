using System.IdentityModel.Tokens.Jwt;
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

            var role = await _userManager.AddToRoleAsync(user, "User");
            if(!role.Succeeded)
                throw new InvalidOperationException("Failed to assign role to user.");

            var accessToken = await GenerateTokenString(user);
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

            var accessToken = await GenerateTokenString(user);
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
            var now = DateTime.UtcNow;
            var refreshLifeTime = TimeSpan.FromHours(12);
            var freeWindow = TimeSpan.FromSeconds(30);

            var user = await _userManager.Users.SingleOrDefaultAsync(u =>
                 u.RefreshToken == refreshToken || u.PreviousRefreshToken == refreshToken);
            if (user == null)
                throw new InvalidOperationException("Invalid refresh token.");

            if(user.RefreshToken == refreshToken)
            {
                if(user.RefreshTokenExpiryTime <= now) 
                    throw new InvalidOperationException("Refresh token expired.");

                var newAccessToken = await GenerateTokenString(user);

                user.PreviousRefreshToken = user.RefreshToken;
                user.PreviousRefreshTokenExpiryTime = now.Add(freeWindow);

                var newRefreshToken = GenerateRefreshToken();
                var newRefreshExpires = now.Add(refreshLifeTime);

                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = newRefreshExpires;
                user.LastLogInAt = now;
                await _userManager.UpdateAsync(user);

                return new AuthTokens
                    {
                        IsLoggedIn = true,
                        AccessToken = newAccessToken,
                        RefreshToken = newRefreshToken,
                        RefreshTokenExpiryTime = newRefreshExpires,
                        Username = user.UserName!
                    };
            }

            if (user.PreviousRefreshToken == refreshToken)
            {
                if (user.PreviousRefreshTokenExpiryTime <= now)
                    throw new InvalidOperationException("Refresh token expired.");

                var newAccessToken = await GenerateTokenString(user);

                return new AuthTokens
                {
                    IsLoggedIn = true,
                    AccessToken = newAccessToken,
                    RefreshToken = user.RefreshToken!,
                    RefreshTokenExpiryTime = user.RefreshTokenExpiryTime!.Value,
                    Username = user.UserName!
                };
            }

            throw new InvalidOperationException("Invalid refresh token.");
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

            var roles = await _userManager.GetRolesAsync(user);
            if(roles.Count == 0) 
                throw new InvalidOperationException("User has no roles assigned.");

            return new ProfileResponseDto
            {
                Username = user.UserName!,
                Email = user.Email!,
                Roles = roles
            };
        }

        private async Task<string> GenerateTokenString(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

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
