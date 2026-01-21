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
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(request.Username);

            if(user == null) return Unauthorized("Invalid username or password");

            var result  = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if(!result.Succeeded) return Unauthorized("Invalid username or password");

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(
                new NewUserDto
                {
                    Username = user.UserName ?? "",
                    Email = user.Email ?? "",
                    Token = _tokenService.CreateToken(user, roles)

                }
            );


        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var appUser = new AppUser
            {
                UserName = request.Username,
                Email = request.Email
            };

            var createUser = await _userManager.CreateAsync(appUser, request.Password);
            if (!createUser.Succeeded)
                return BadRequest(createUser.Errors);

            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
            if (!roleResult.Succeeded)
                return StatusCode(500, roleResult.Errors);

            var roles = await _userManager.GetRolesAsync(appUser);

            return Ok(new NewUserDto
            {
                Username = appUser.UserName ?? "",
                Email = appUser.Email ?? "",
                Token = _tokenService.CreateToken(appUser, roles) 
            });
        }



        
    }
}