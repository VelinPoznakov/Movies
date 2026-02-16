using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie.Entities
{
    public class MovieEntity
    {
        [Key]
        public long Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        public string TimeLong { get; set; } = string.Empty;

        [ForeignKey("DirectorId")]
        public Director? Director { get; set; }
        public long? DirectorId { get; set; }

        public DateTime ReleaseDate { get; set; }

        [ForeignKey("GenreId")]
        public long? GenreId { get; set; }
        public Genre? Genre { get; set; }
        public List<Rating> Ratings { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public string? UserId { get; set; }
        public AppUser? User { get; set; }
        public List<Comment> Comments { get; set; } = new();
    }
}
