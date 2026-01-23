using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movie.Dtos.Comments.Request;
using Movie.Entities;

namespace Movie.Dtos.Comments.Request
{
    public class CreateCommentsRequestDto
    {
      public string Content { get; set; } = string.Empty;
      public long? MovieId { get; set; }
    }
}

public class CreateCommentsRequestDtoProfile: Profile
{
  public CreateCommentsRequestDtoProfile()
  {
    CreateMap<CreateCommentsRequestDto, Comments>()
        .ForMember(c => c.CreatedAt, opt => opt.Ignore())
        .ForMember(c => c.UpdatedAt, opt => opt.Ignore())
        .ForMember(c => c.Id, opt => opt.Ignore());
  }
}