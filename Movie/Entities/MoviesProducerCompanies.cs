using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Movie.Entities;

[PrimaryKey(nameof(MovieId), nameof(ProducerCompanyId))]
public class MoviesProducerCompanies
{
    [ForeignKey(nameof(Movie))]
    public int MovieId { get; set; }
    public virtual MovieEntity Movie { get; set; } = null!;
    
    [ForeignKey(nameof(ProducerCompany))]
    public int ProducerCompanyId { get; set; }
    public virtual ProducerCompany ProducerCompany { get; set; } = null!;
}