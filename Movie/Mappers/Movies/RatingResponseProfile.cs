using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movie.Dtos.Rating;
using Movie.Entities;

namespace Movie.Mappers.Movies
{
    public class RatingResponseProfile: Profile
    {
        public RatingResponseProfile()
        {
            CreateMap<Rating, RatingResponse>();
        }
        
    }
}