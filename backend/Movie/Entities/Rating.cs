using System.ComponentModel.DataAnnotations.Schema;

namespace Movie.Entities
{
    public class Rating: BaseEntity
    {
        [ForeignKey(nameof(Movie))]
        public long MovieId { get; set; }
        public virtual MovieEntity Movie { get; set; } = null!;

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public virtual AppUser User { get; set; } = null!;
 
        public double Value { get; set; }
    }
}
