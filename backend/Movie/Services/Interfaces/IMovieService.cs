
using Movie.Dtos.Movies;
using Movie.Repositories.Interfaces;

namespace Movie.Services.Interfaces
{
    public interface IMovieService
    {
        Task<MovieDetailsResponseDto> GetMovieDetailsAsync(long id);
    }
    
    
}
