using System.ComponentModel.DataAnnotations;

using static Movie.Common.EntitiesConstants.ProducerCompany;

namespace Movie.Entities;

public class ProducerCompany: BaseTMDB
{
    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; } = null!;

    [MaxLength(LogoPathMaxLength)]
    public string? LogoPath { get; set; }

    public virtual ICollection<MoviesProducerCompanies>  MoviesProducerCompanies { get; set; }
        = new HashSet<MoviesProducerCompanies>();
}
