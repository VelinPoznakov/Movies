using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Movie.Entities;

namespace Movie.Data
{
    public class MovieDbContext: IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {
            
        }

        public virtual DbSet<MovieEntity> Movies { get; set; } = null!;

        public virtual DbSet<Genre> Genres { get; set; } = null!;
        
        public virtual DbSet<Director> Directors { get; set; } = null!;
        
        public virtual DbSet<AppUser> AppUsers { get; set; } = null!;
        
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        
        public virtual DbSet<Rating> Ratings { get; set; } = null!;

        public virtual DbSet<ProducerCompany> ProducerCompanies { get; set; } = null!;

        public virtual DbSet<MoviesDirectors> MoviesDirectors { get; set; } = null!;

        public virtual DbSet<MoviesProducerCompanies> MoviesProducerCompanies { get; set; } = null!;

        public virtual DbSet<MoviesGenres> MoviesGenres { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(MovieDbContext).Assembly);
            
            base.OnModelCreating(builder);
        }
    }
}
