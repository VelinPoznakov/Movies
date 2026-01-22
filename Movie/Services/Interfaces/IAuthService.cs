using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movie.Dtos.Auth;

namespace Movie.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDto> RegisterUser(RegisterDto request);
        Task<TokenDto> LoginUser(LoginUserDto request);
        Task<TokenDto> RefreshToken(RefreshTokenDto request);
    }
}