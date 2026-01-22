using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Movie.Entities;

namespace Movie.Data
{
    public class MovieDbContext: IdentityDbContext<AppUser>
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {
        }
        public DbSet<MovieEntity> Movies => Set<MovieEntity>();

        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Director> Directors => Set<Director>();
        public DbSet<AppUser> AppUsers => Set<AppUser>();

    }
}
