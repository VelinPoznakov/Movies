using Microsoft.EntityFrameworkCore;
using Movie.Entities;

namespace Movie.Data
{
    public class MovieDbContext: DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {
        }
        public DbSet<MovieEntity> Movies => Set<MovieEntity>();

        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Director> Directors => Set<Director>();
    }
}
