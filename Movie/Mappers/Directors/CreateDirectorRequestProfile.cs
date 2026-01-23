using AutoMapper;
using Movie.Dtos.Director.Request;
using Movie.Entities;

namespace Movie.Mappers.Directors
{
    public class CreateDirectorRequestProfile: Profile
    {
        public CreateDirectorRequestProfile()
        {
            CreateMap<CreateDirectorRequestDto, Director>()
                .ForMember(d => d.Id, opt => opt.Ignore());
        }
    }
}