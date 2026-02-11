using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Dtos.Auth
{
    public class SessionDto
    {
        public bool IsLoggedIn { get; set; }
        public string? Token { get; set; } = string.Empty;
        public ProfileResponseDto? User { get; set; }
    }
}