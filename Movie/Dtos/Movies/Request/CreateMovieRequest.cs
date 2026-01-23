using AutoMapper;
using Movie.Dtos.Movies.Request;
using Movie.Entities;

namespace Movie.Dtos.Movies.Request
{
    public class CreateMovieRequest
    {
        public string Title { get; set; } = string.Empty;
        public string TimeLong { get; set; } = string.Empty;

        public string DirectorName { get; set; } = string.Empty;
        public string GenreName { get; set; } = string.Empty;
        public string ReleaseDate { get; set; } = string.Empty;

    }
}
