using AutoMapper;
using Movie.Dtos.Genre.Request;
using Movie.Entities;

namespace Movie.Dtos.Genre.Request
{
    public class UpdateGenreRequestDto
    {
        public string Name { get; set; } = string.Empty;
    }
}

public class UpdateGenreRequestProfile: Profile
{
    public UpdateGenreRequestProfile()
    {
        CreateMap<UpdateGenreRequestDto, Genre>()
            .ForMember(d => d.Id, opt => opt.Ignore());
    }
}