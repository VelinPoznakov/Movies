using Movie.Dtos.TMDB.Movie;
using Movie.Dtos.TMDB.Person;
using Movie.Dtos.TMDB.ProductionCompany;

namespace Movie.ExternalSystems.Contracts;

public interface ITMDBHandler
{
    Task<MovieCollectionResponseDto?> DiscoverMoviesAsync(DateOnly fromDate, DateOnly toDate, int page = 1);
    Task<MovieDetailsResponseDto?> GetMovieDetailsAsync(long tmdbId, bool includeCast = false);
    Task<PersonResponseDto?> GetPersonAsync(long tmdbId);
    Task<CompanyDetailsResponseDto?> GetCompanyAsync(long tmdbId);
    Task<Stream?> GetImageStreamAsync(string imagePath);
}