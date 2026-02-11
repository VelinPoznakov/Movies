using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Dtos.Auth;
using Movie.Services.Interfaces;

namespace Movie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController: ControllerBase
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        private void SetRefreshTokenCookie(string token, DateTime expires)
        {
            Response.Cookies.Append(
                "refreshToken",
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.Strict,
                    Expires = expires,
                    Path = "/api" // allow session + refresh endpoints to read it
                }
                
            );
        }

        private void DeleteRefreshTokenCookie()
        {
            Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                Path = "/api"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto request)
        {
            var tokens = await _authService.LoginUser(request);
            if (tokens == null) return Unauthorized("Invalid credentials");

            SetRefreshTokenCookie(tokens.RefreshToken, tokens.RefreshTokenExpiryTime);

            return Ok(
                new TokenDto
                {
                    IsLoggedIn = true,
                    Token = tokens.AccessToken
                }
            );
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            var tokens = await _authService.RegisterUser(request);
            if(tokens == null) return Unauthorized();

            SetRefreshTokenCookie(tokens.RefreshToken, tokens.RefreshTokenExpiryTime);

            return Ok(
                new TokenDto
                {
                    IsLoggedIn = true,
                    Token = tokens.AccessToken
                }
            );
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null) return Unauthorized("No refresh token provided");

            var tokens = await _authService.RefreshToken(refreshToken);
            if (tokens == null) return Unauthorized("Invalid refresh token");

            SetRefreshTokenCookie(tokens.RefreshToken, tokens.RefreshTokenExpiryTime);

            return Ok(
                new TokenDto
                {
                    IsLoggedIn = true,
                    Token = tokens.AccessToken
                }
            );
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var username = User?.Identity?.Name;
            if(username == null) return Unauthorized();

            await _authService.LogoutUserAsync(username);

            DeleteRefreshTokenCookie();

            return Ok(new { Message = "Logged out successfully" });
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var username = User?.Identity?.Name;
            if (username == null) return Unauthorized();

            var result = await _authService.GetProfileAsync(username);
            
            return result == null ? NotFound() : Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("session")]
        public async Task<IActionResult> GetSession()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                var username = User.Identity?.Name;
                if (string.IsNullOrWhiteSpace(username)) return Ok(null);

                var user = await _authService.GetProfileAsync(username);
                return Ok(new SessionDto { IsLoggedIn = true, Token = null, User = user });
            }

            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrWhiteSpace(refreshToken))
                return Ok(null);

            var tokens = await _authService.RefreshToken(refreshToken);

            SetRefreshTokenCookie(tokens.RefreshToken, tokens.RefreshTokenExpiryTime);


            var principalName = new JwtSecurityTokenHandler()
                .ReadJwtToken(tokens.AccessToken)
                .Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrWhiteSpace(principalName)) return Ok(null);

            var profile = await _authService.GetProfileAsync(principalName);

            return Ok(new SessionDto { IsLoggedIn = true, Token = tokens.AccessToken, User = profile });
        }

        
    }
}
