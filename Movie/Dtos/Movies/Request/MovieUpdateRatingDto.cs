using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Dtos.Movies.Request
{
    public class MovieUpdateRatingDto
    {
        public long MovieId { get; set; }
        public string Username { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}