using System.ComponentModel.DataAnnotations;

using static Movie.Common.EntitiesConstants.Genre;

namespace Movie.Entities
{
    public class Genre: BaseTMDB
    {
        [Required] 
        [MaxLength(NameMaxLength)] 
        public string Name { get; set; } = null!;
        
        public virtual ICollection<MoviesGenres> MoviesGenres { get; set; }
            = new List<MoviesGenres>();
    }
}
