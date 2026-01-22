using Microsoft.AspNetCore.Identity;

namespace Movie.Entities
{
    public class AppUser: IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogInAt { get; set; }
        public List<MovieEntity> Movies { get; set; } = new List<MovieEntity>();
        public List<Genre> Genres { get; set; } = new List<Genre>();
        public List<Director> Directors { get; set; } = new List<Director>();

                 
    }
}