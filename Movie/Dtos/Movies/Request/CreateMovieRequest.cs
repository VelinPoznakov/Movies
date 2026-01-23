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

public class CreateMovieRequestProfile : Profile
{
    public CreateMovieRequestProfile()
    {
        CreateMap<CreateMovieRequest, MovieEntity>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Director, opt => opt.Ignore())
            .ForMember(d => d.DirectorId, opt => opt.Ignore())
            .ForMember(d => d.Genre, opt => opt.Ignore())
            .ForMember(d => d.GenreId, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.LastUpdatedAt, opt => opt.Ignore())
            .ForMember(d => d.ReleaseDate, opt => opt.Ignore())
            .ForMember(d => d.Comments, opt => opt.Ignore());
    }
}

