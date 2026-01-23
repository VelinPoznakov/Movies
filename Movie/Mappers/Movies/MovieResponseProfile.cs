using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movie.Dtos.Movies.Response;
using Movie.Entities;

namespace Movie.Mappers.Movies
{
    public class MovieResponseProfile : Profile
    {
        public MovieResponseProfile()
        {
            CreateMap<MovieEntity, MovieResponseDto>();
        }
    }
}