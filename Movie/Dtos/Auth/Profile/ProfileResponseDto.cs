using AutoMapper;
using Movie.Dtos.Auth;
using Movie.Dtos.Movies.Response;
using Movie.Entities;

namespace Movie.Dtos.Auth
{
    public class ProfileResponseDto
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}