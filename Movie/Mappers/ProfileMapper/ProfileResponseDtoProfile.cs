using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movie.Dtos.Auth;
using Movie.Entities;

namespace Movie.Mappers.ProfileMapper
{
    public class ProfileResponseDtoProfile: Profile
    {
        public ProfileResponseDtoProfile()
        {
            CreateMap<AppUser, ProfileResponseDto>();
        }
    }
}