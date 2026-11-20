using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movie.Entities;

[PrimaryKey(nameof(MovieId), nameof(ProducerCompanyId))]
public class MoviesProducerCompanies
{
    [ForeignKey(nameof(Movie))]
    public long MovieId { get; set; }
    public virtual MovieEntity Movie { get; set; } = null!;
    
    [ForeignKey(nameof(ProducerCompany))]
    public long ProducerCompanyId { get; set; }
    public virtual ProducerCompany ProducerCompany { get; set; } = null!;
}