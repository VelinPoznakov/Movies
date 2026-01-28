using Microsoft.EntityFrameworkCore;
using Movie.Data;
using Movie.Entities;
using Movie.Repositories.Interfaces;

namespace Movie.Repositories
{
  public class CommentsRepository : ICommentsRepository
  {
    private readonly MovieDbContext _context;

    public CommentsRepository(MovieDbContext context)
    {
      _context = context;
    }
    
    public async Task CreateCommentAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(Comment comment)
    {
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }

    public async Task<Comment?> GetCommentByIdAsync(long id)
    {
        return await _context.Comments
            .Include(c => c.Movie)
                .ThenInclude(m => m!.Director)
            .Include(c => c.Movie)
                .ThenInclude(m => m!.Genre)
            .FirstOrDefaultAsync(c => c.Id == id);
    }


    public async Task<List<Comment>> GetCommentsByMovieIdAsync(long movieId)
    {
        return await _context.Comments
            .Include(c => c.Movie)
                .ThenInclude(m => m!.Director)
            .Include(c => c.Movie)
                .ThenInclude(m => m!.Genre)
            .Where(c => c.MovieId == movieId)
            .ToListAsync();
    }


    public async Task UpdateCommentAsync(Comment comment)
    {
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
    }
  }
}