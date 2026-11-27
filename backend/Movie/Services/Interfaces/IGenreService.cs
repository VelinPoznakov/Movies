using Movie.Dtos.Genre;
using Movie.Dtos.TMDB.Genre;

namespace Movie.Services.Interfaces;

public interface IGenreService
{
    Task<GenreCollectionDto> GetAllGenresAsync(int page);
    
    Task<GenreWithMoviesDto> GetGenreDetailsWithMoviesAsync(long id, int page);
}