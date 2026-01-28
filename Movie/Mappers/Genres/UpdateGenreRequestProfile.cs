using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movie.Dtos.Genre.Request;
using Movie.Entities;

namespace Movie.Mappers.Genres
{
    public class UpdateGenreRequestProfile: Profile
    {
        public UpdateGenreRequestProfile()
        {
            CreateMap<UpdateGenreRequestDto, Genre>()
                .ForMember(d => d.Id, opt => opt.Ignore());
        }
    }
}