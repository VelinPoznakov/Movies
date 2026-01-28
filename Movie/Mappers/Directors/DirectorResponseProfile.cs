using AutoMapper;
using Movie.Dtos.Director.Response;
using Movie.Entities;

namespace Movie.Mappers.Directors
{
    public class DirectorResponseProfile : Profile
    {
        public DirectorResponseProfile()
        {
            CreateMap<Director, DirectorReasponse>();
        }
    }
}