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
    
    public async Task CreateCommentAsync(Comments comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(Comments comment)
    {
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }

    public async Task<Comments?> GetCommentByIdAsync(long id)
    {
        return await _context.Comments.FindAsync(id);
    }

    public async Task<List<Comments>> GetCommentsByMovieIdAsync(long movieId)
    {
        return await _context.Comments
            .Where(c => c.MovieId == movieId)
            .ToListAsync();
    }

    public async Task UpdateCommentAsync(Comments comment)
    {
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
    }
  }
}