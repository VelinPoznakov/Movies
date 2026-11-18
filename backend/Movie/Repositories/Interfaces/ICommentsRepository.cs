using Movie.Entities;

namespace Movie.Repositories.Interfaces
{
    public interface ICommentsRepository
    {
        Task CreateCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(Comment comment);
        Task<Comment?> GetCommentByIdAsync(long id);
        Task<List<Comment>> GetCommentsByMovieIdAsync(long movieId);
    }
}