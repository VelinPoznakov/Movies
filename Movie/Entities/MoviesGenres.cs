using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movie.Entities;

[PrimaryKey(nameof(MovieId), nameof(GenreId))]
public class MoviesGenres
{
    [ForeignKey(nameof(Movie))]
    public int MovieId { get; set; }
    public virtual MovieEntity Movie { get; set; } = null!;

    [ForeignKey(nameof(Genre))]
    public int GenreId { get; set; }
    public virtual Genre Genre { get; set; } = null!;
}
