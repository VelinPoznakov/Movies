using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movie.Entities;

[PrimaryKey(nameof(MovieId), nameof(GenreId))]
public class MoviesGenres
{
    [ForeignKey(nameof(Movie))]
    public long MovieId { get; set; }
    public virtual MovieEntity Movie { get; set; } = null!;

    [ForeignKey(nameof(Genre))]
    public long GenreId { get; set; }
    public virtual Genre Genre { get; set; } = null!;
}
