using System.ComponentModel.DataAnnotations;

namespace Movie.Entities
{
    public class Director
    {
        [Key]
        public long Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public List<MovieEntity>? MovieEntitys { get; set; }
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
    }
}
