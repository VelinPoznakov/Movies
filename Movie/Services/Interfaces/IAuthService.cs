using Movie.Dtos.Auth;

namespace Movie.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthTokens> RegisterUser(RegisterDto request);
        Task<AuthTokens> LoginUser(LoginUserDto request);
        Task<AuthTokens> RefreshToken(string refreshToken);
        Task<ProfileResponseDto> GetProfileAsync(string username);
        Task LogoutUserAsync(string username);
    }
}