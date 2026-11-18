using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie.Entities;

[PrimaryKey(nameof(MovieId), nameof(DirectorId))]
public class MoviesDirectors
{
    [ForeignKey(nameof(Director))]
    public int DirectorId { get; set; }
    public virtual Director Director { get; set; } = null!;

    [ForeignKey(nameof(MovieEntity))]
    public int MovieId { get; set; }
    public virtual MovieEntity MovieEntity { get; set; } = null!;
}