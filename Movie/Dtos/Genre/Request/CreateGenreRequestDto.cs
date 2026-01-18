using AutoMapper;
using Movie.Dtos.Genre.Request;
using Movie.Entities;

namespace Movie.Dtos.Genre.Request
{
    public class CreateGenreRequestDto
    {
        public string Name { get; set; } = string.Empty;
    }
}

public class CreateGenreRequestProfile : Profile
{
    public CreateGenreRequestProfile()
    {
        CreateMap<CreateGenreRequestDto, Genre>()
            .ForMember(d => d.Id, opt => opt.Ignore());
    }
}
