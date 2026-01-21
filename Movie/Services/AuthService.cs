using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
            return new TokenDto{ Token = token };
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

            return new TokenDto{ Token = token };
        }

        public string GenerateTockenString(string username)
        {
            var claims = new List<Claim>
            {
              new Claim(ClaimTypes.Name, username),
              new Claim(ClaimTypes.Role, "User"),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? ""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims:claims,
                expires: DateTime.Now.AddMinutes(60),
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                signingCredentials: credentials
                );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
        
    }
}