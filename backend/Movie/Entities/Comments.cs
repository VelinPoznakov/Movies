using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static Movie.Common.EntitiesConstants.Comment;

namespace Movie.Entities
{
    public class Comment: BaseEntity
    {
        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; } = null!;

        [ForeignKey(nameof(Movie))]
        public int MovieId { get; set; }
        public virtual MovieEntity Movie { get; set; } = null!;

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public virtual AppUser User { get; set; } = null!;
    }
}
