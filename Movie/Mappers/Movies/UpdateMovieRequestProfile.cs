using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movie.Dtos.Movies.Request;
using Movie.Entities;

namespace Movie.Mappers.Movies
{

    public class UpdateMovieRequestProfile : Profile
    {
        public UpdateMovieRequestProfile()
        {
            CreateMap<UpdateMovieRequestDto, MovieEntity>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Director, opt => opt.Ignore())
                .ForMember(d => d.DirectorId, opt => opt.Ignore())
                .ForMember(d => d.Genre, opt => opt.Ignore())
                .ForMember(d => d.GenreId, opt => opt.Ignore())
                .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                .ForMember(d => d.LastUpdatedAt, opt => opt.Ignore())
                .ForMember(d => d.ReleaseDate, opt => opt.Ignore())
                .ForMember(d => d.Comments, opt => opt.Ignore());
        }
    }
}