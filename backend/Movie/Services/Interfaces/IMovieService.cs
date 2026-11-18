using Movie.Dtos.Genre.Request;
using Movie.Dtos.Movies.Request;
using Movie.Dtos.Movies.Response;

namespace Movie.Services.Interfaces
{
    public interface IMovieService
    {
        Task<List<MovieResponseDto>> GetAllMovies();
        Task<MovieResponseDto> GetMovieById(long id);
        Task<MovieResponseDto> CreateMovie(CreateMovieRequest dto, string userId);
        Task<MovieResponseDto> UpdateMovie(long id, UpdateMovieRequestDto dto);
        Task DeleteMovie(long id);
        Task<MovieResponseDto> UpdateMovieRating(long id, MovieUpdateRatingDto dto);
    }
}
