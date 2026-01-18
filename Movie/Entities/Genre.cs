using System.ComponentModel.DataAnnotations;

namespace Movie.Entities
{
    public class Genre
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Movie>? Movies { get; set; }
    }
}