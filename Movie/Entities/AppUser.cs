using Microsoft.AspNetCore.Identity;

namespace Movie.Entities
{
    public class AppUser: IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string? PreviousRefreshToken { get; set; }
        public DateTime? PreviousRefreshTokenExpiryTime { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogInAt { get; set; }
        public List<MovieEntity> Movies { get; set; } = new List<MovieEntity>();
        public List<Comment> Comments { get; set; } = new();
                 
    }
}