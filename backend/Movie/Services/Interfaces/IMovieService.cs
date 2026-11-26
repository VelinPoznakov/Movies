using Movie.Dtos.Movies;

namespace Movie.Services.Interfaces
{
    public interface IMovieService
    {
        Task<MovieDetailsResponseDto> GetMovieDetailsAsync(long id);
        Task<MovieCollectionResponseDto> GetMovieCollectionsAsync(int page);
    }
    
    
}
