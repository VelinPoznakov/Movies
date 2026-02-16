using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movie.Data;
using Movie.Entities;
using Movie.Repositories.Interfaces;

namespace Movie.Repositories
{
  public class RatingRepository : IRatingRepository
  {
    private readonly MovieDbContext _context;

    public RatingRepository(MovieDbContext context)
    {
      _context = context;
    }

    public async Task AddMovieRatingAsync(Rating rating)
    {
      await _context.Ratings.AddAsync(rating);
      await _context.SaveChangesAsync();
    }

    public async Task<Rating?> GetMovieRatingByMovieAndUserAsync(long movieId, string userId)
    {
      var rating = await _context.Ratings.FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);
      return rating;
    }

    public async Task UpdateMovieRatingAsync(Rating rating)
    {
      _context.Ratings.Update(rating);
      await _context.SaveChangesAsync();
    }
  }
}