using AutoMapper;
using Movie.Dtos.Genre.Response;
using Movie.Entities;

namespace Movie.Dtos.Genre.Response
{
    public class GenreResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}

public class GenreResponseProfile : Profile
{
    public GenreResponseProfile()
    {
        CreateMap<Genre, GenreResponse>();
    }
}