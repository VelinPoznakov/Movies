using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Dtos.Auth
{
    public class AuthTokens
    {
        public bool IsLoggedIn { get; set; }
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string Username { get; set; } = string.Empty;
    }
}