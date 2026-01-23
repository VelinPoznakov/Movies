using AutoMapper;
using Movie.Dtos.Comments.Response;
using Movie.Dtos.Director.Response;
using Movie.Dtos.Genre.Response;
using Movie.Dtos.Movies.Request;
using Movie.Dtos.Movies.Response;
using Movie.Entities;

namespace Movie.Dtos.Movies.Response
{
    public class MovieResponseDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string TimeLong { get; set; } = string.Empty;
        public DirectorReasponse? Director { get; set; }
        public DateTime ReleaseDate { get; set; }
        public GenreResponse? Genre { get; set; }
        public double Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string UserId { get; set; } = string.Empty;
        public List<CommentResponseDto>? Comments { get; set; }
    }
}
