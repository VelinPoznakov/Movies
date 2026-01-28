using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movie.Dtos.Comments.Response;
using Movie.Dtos.Movies.Response;
using Movie.Entities;
using Movie.Repositories;

namespace Movie.Dtos.Comments.Response
{
    public class CommentResponseDto
    {
        public long Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
