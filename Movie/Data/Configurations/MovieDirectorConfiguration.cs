using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Entities;

namespace Movie.Data.Configurations;

public class MoviesDirectorsConfiguration : IEntityTypeConfiguration<MoviesDirectors>
{
    public void Configure(EntityTypeBuilder<MoviesDirectors> builder)
    {
        builder
            .HasOne(md => md.MovieEntity)
            .WithMany(m => m.MoviesDirectors)
            .HasForeignKey(md => md.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(md => md.Director)
            .WithMany(d => d.MoviesDirectors)
            .HasForeignKey(md => md.DirectorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}