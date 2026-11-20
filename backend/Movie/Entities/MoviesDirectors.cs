using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie.Entities;

[PrimaryKey(nameof(MovieId), nameof(DirectorId))]
public class MoviesDirectors
{
    [ForeignKey(nameof(Director))]
    public long DirectorId { get; set; }
    public virtual Director Director { get; set; } = null!;

    [ForeignKey(nameof(MovieEntity))]
    public long MovieId { get; set; }
    public virtual MovieEntity MovieEntity { get; set; } = null!;
}