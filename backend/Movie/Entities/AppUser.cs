using Microsoft.AspNetCore.Identity;

namespace Movie.Entities
{
    public class AppUser: IdentityUser<Guid>
    {
        
        public string? RefreshToken { get; set; }
        
        public DateTime? RefreshTokenExpiryTime { get; set; }
        
        public string? PreviousRefreshToken { get; set; }
        
        public DateTime? PreviousRefreshTokenExpiryTime { get; set; }

        public string Name { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogInAt { get; set; }
        
        public virtual ICollection<MovieEntity> Movies { get; set; } 
            = new List<MovieEntity>();
        
        public virtual ICollection<Comment> Comments { get; set; } 
            = new List<Comment>();
                 
    }
}