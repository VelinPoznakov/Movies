using Movie.Entities;

namespace Movie.Repositories.Interfaces
{
    public interface ICommentsRepository
    {
        Task CreateCommentAsync(Comments comment);
        Task UpdateCommentAsync(Comments comment);
        Task DeleteCommentAsync(Comments comment);
        Task<Comments?> GetCommentByIdAsync(long id);
        Task<List<Comments>> GetCommentsByMovieIdAsync(long movieId);
    }
}