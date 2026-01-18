using AutoMapper;
using Movie.Dtos.Director.Request;
using Movie.Entities;

namespace Movie.Dtos.Director.Request
{
    public class UpdateDirectorRequestDto
    {
        public string Name { get; set; } = string.Empty;
    }
}

public class UpdateDirectorRequestProfile: Profile
{
    public UpdateDirectorRequestProfile()
    {
        CreateMap<UpdateDirectorRequestDto, Director>()
            .ForMember(d => d.Id, opt => opt.Ignore());
    }
}