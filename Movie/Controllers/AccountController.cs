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
            if (res == null) return BadRequest("failed");
            return Ok(res);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            var res = await _authService.RegisterUser(request);
            if (res == null) return BadRequest("failed");
            return Ok(res);
        }


        


        
    }
}