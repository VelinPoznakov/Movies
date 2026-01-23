using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movie.Dtos.Director.Request;
using Movie.Entities;

namespace Movie.Mappers.Directors
{
    public class UpdateDirectorRequestProfile: Profile
    {
        public UpdateDirectorRequestProfile()
        {
            CreateMap<UpdateDirectorRequestDto, Director>()
                .ForMember(d => d.Id, opt => opt.Ignore());
        }
    }
}