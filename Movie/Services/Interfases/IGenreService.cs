using Movie.Dtos.Genre.Request;
using Movie.Dtos.Genre.Response;


namespace Movie.Services.Interfases
{
    public interface IGenreService
    {
        Task<List<GenreResponse>> GetAllGenreAsync();
        Task<GenreResponse> GetGenreByIdAsync(int id);
        Task<GenreResponse> UpdateGenreAsync(int id, UpdateGenreRequestDto dto);
        Task CreateGenreAsync(CreateGenreRequestDto dto);
        Task DeleteGenreAsync(int id);
    }
}