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
    }
}
