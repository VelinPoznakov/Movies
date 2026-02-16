using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movie.Entities;

namespace Movie.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task UpdateMovieRatingAsync(Rating rating);
        Task AddMovieRatingAsync(Rating rating);
        Task<Rating?> GetMovieRatingByMovieAndUserAsync(long movieId, string userId);
    }
}