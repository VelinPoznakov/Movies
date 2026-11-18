using Movie.Dtos.TMDB.Movie;

namespace Movie.ExternalSystems.Contracts;

public interface ITMDBHandler
{
    Task<MovieCollectionResponseDto> DiscoverMoviesAsync(DateOnly fromDate, int page = 1);
    Task<MovieDetailsResponseDto> GetMovieDetailsAsync(long tmdbId, bool includeCast = false);
    Task<byte[]?> GetImageAsync(string imageUrl);
}