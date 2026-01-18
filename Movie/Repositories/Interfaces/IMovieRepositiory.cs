
using Movie.Entities;

namespace Movie.Repositories.Interfaces
{
    public interface IMovieRepositiory
    {
        Task<List<MovieEntity>> GetMoviesAsync();
        Task<MovieEntity?> GetMovieAsync(long id);
        Task AddAsync(MovieEntity movie);
        Task Delete(MovieEntity movie);
        Task UpdateAsync(MovieEntity movie);
    }
}
