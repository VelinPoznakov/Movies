using Microsoft.EntityFrameworkCore;
using Movie.Data;
using Movie.Entities;
using Movie.Repositories.Interfaces;

namespace Movie.Repositories
{
    public class MovieRepositiory: IMovieRepositiory
    {
        private readonly MovieDbContext _context;

        public MovieRepositiory(MovieDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MovieEntity movie)
        {
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

        }

        public async Task Delete(MovieEntity movie)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }

        public async Task<MovieEntity?> GetMovieAsync(long id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public async Task<List<MovieEntity>> GetMoviesAsync()
        {
            return await _context.Movies
                .Include(m => m.Director)
                .Include(m => m.Genre)
                .Include(m => m.User)
                .Include(m => m.Comments)
                    .ThenInclude(c => c.User)
                .ToListAsync();
        }

        public async Task UpdateAsync(MovieEntity movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }
    }
}
