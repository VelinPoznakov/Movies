using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Entities;

namespace Movie.Data.Configurations;

public class MoviesProducerCompaniesConfiguration: IEntityTypeConfiguration<MoviesProducerCompanies>
{
    public void Configure(EntityTypeBuilder<MoviesProducerCompanies> builder)
    {
        builder
            .HasOne(m => m.Movie)
            .WithMany(m => m.MoviesProducerCompanies)
            .HasForeignKey(m => m.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(pc => pc.ProducerCompany)
            .WithMany(m => m.MoviesProducerCompanies)
            .HasForeignKey(pc => pc.ProducerCompanyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}