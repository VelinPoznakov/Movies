using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Entities
{
    public class Rating
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("MovieId")]
        public long? MovieId { get; set; }
        public MovieEntity? Movie { get; set; }

        public string? UserId { get; set; }
        public AppUser? User { get; set; }

        public double Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


    }
}