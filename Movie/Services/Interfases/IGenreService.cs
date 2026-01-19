using Movie.Dtos.Genre.Request;
using Movie.Dtos.Genre.Response;


namespace Movie.Services.Interfases
{
    public interface IGenreService
    {
        Task<List<GenreResponse>> GetAllGenreAsync();
        Task<GenreResponse> GetGenreByIdAsync(long id);
        Task<GenreResponse> UpdateGenreAsync(long id, UpdateGenreRequestDto dto);
        Task<GenreResponse> CreateGenreAsync(CreateGenreRequestDto dto);
        Task DeleteGenreAsync(long id);
    }
}