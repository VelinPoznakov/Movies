using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movie.Entities;

namespace Movie.Dtos.Comments.Request
{
    public class UpdateCommentRequestDto
    {
        public string Content { get; set; } = string.Empty;
    }
}

public class UpdateCommentRequestDtoProfile: Profile
{
  public UpdateCommentRequestDtoProfile()
  {
    CreateMap<UpdateDirectorRequestProfile, Comments>()
        .ForMember(c => c.CreatedAt, opt => opt.Ignore())
        .ForMember(c => c.UpdatedAt, opt => opt.Ignore())
        .ForMember(c => c.Id, opt => opt.Ignore())
        .ForMember(c => c.MovieId, opt => opt.Ignore())
        .ForMember(c => c.UserId, opt => opt.Ignore());
  }
}