using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Movie.Entities.Enums;
using static Movie.Common.EntitiesConstants.MovieEntity;

namespace Movie.Entities
{
    public class MovieEntity: BaseTMDB
    {
        [Required] 
        [MaxLength(TitleMaxLength)] 
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public int TimeLong { get; set; }

        public double Budget { get; set; }

        [MaxLength(ImdbIdMaxLength)]
        public string? ImdbId { get; set; }

        [Required]
        public MovieStatus Status { get; set; }

        [MaxLength(PosterBlobNameMaxLength)]
        public string? PosterBlobName { get; set; }

        [MaxLength(PosterBlobNameMaxLength)]
        public string? BackDroopPoster { get; set; }
        
        [Required]
        public DateTime ReleaseDate { get; set; }
        
        [Required]
        public bool IsAdult { get; set; }
        
        public virtual ICollection<MoviesProducerCompanies> MoviesProducerCompanies { get; set; }
            = new List<MoviesProducerCompanies>();

        public virtual ICollection<MoviesDirectors>  MoviesDirectors { get; set; }
            = new List<MoviesDirectors>();

        public virtual ICollection<MoviesGenres> MoviesGenres { get; set; }
            = new List<MoviesGenres>();

        [ForeignKey(nameof(User))]
        public Guid? UserId { get; set; }
        public virtual AppUser? User { get; set; }
        
        public virtual ICollection<Comment> Comments { get; set; } 
            = new List<Comment>();
        
        public virtual ICollection<Rating> Ratings { get; set; }
            = new List<Rating>();
    }
}
