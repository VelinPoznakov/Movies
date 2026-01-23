using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movie.Dtos.Genre.Response;
using Movie.Entities;

namespace Movie.Mappers.Genres
{
    public class GenreResponseProfile : Profile
    {
        public GenreResponseProfile()
        {
            CreateMap<Genre, GenreResponse>();
        }
    }
}