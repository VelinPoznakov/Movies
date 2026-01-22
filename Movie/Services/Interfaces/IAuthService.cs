using Movie.Dtos.Auth;

namespace Movie.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDto> RegisterUser(RegisterDto request);
        Task<TokenDto> LoginUser(LoginUserDto request);
        Task<TokenDto> RefreshToken(RefreshTokenDto request);
        Task LogoutUserAsync(string username);
    }
}