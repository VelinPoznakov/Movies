using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Movie.Entities
{
    public class Comment
    {
        [Key]
        public long Id { get; set; }

        [Required, MaxLength(1000)]
        public string Content { get; set; } = string.Empty;

        [ForeignKey("Movie")]
        public long? MovieId { get; set; }
        public MovieEntity? Movie { get; set; }

        public string? UserId { get; set; }
        public AppUser? User { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


    }
}