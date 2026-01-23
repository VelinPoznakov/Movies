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
