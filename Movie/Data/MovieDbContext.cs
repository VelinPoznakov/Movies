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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                        ConcurrencyStamp = "1"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "2"
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
