using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Entities;

namespace Movie.Data.Configurations;

public class MoviesGenresConfiguration : IEntityTypeConfiguration<MoviesGenres>
{
    public void Configure(EntityTypeBuilder<MoviesGenres> builder)
    {
        builder
            .HasOne(mg => mg.Movie)
            .WithMany(m => m.MoviesGenres)
            .HasForeignKey(mg => mg.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(mg => mg.Genre)
            .WithMany(g => g.MoviesGenres)
            .HasForeignKey(mg => mg.GenreId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
