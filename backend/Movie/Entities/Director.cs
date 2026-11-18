using System.ComponentModel.DataAnnotations;

using static Movie.Common.EntitiesConstants.Director;

namespace Movie.Entities
{
    public class Director: BaseTMDB
    {
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;
        
        [MaxLength(ProfilePathMaxLength)]
        public string? ProfilePath { get; set; }

        public virtual ICollection<MoviesDirectors>  MoviesDirectors { get; set; }
            = new List<MoviesDirectors>();
    }
}
