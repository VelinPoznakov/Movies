using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Dtos.Auth;
using Movie.Entities;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto request)
        {
            var res = await _authService.LoginUser(request);
            if(res.IsLoggedIn) return Ok(res);

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            var res = await _authService.RegisterUser(request);
            if (res == null) return BadRequest("failed");
            return Ok(res);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto request)
        {
            var loginResult = await _authService.RefreshToken(request);
            if(loginResult.IsLoggedIn) return Ok(loginResult);

            return Unauthorized();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var username = User.Identity?.Name;
            if(username == null) return Unauthorized();

            await _authService.LogoutUserAsync(username);
            return Ok(new { Message = "Logged out successfully" });
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var username = User.Identity?.Name;
            if (username == null) return Unauthorized();

            var result = await _authService.GetProfileAsync(username);
            
            return result == null ? NotFound() : Ok(result);
        }


        


        
    }
}