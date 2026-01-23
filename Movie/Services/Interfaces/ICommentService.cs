using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movie.Dtos.Comments.Request;
using Movie.Dtos.Comments.Response;

namespace Movie.Services.Interfaces
{
    public interface ICommentService
    {
        Task<CommentResponseDto> AddCommentAsync(CreateCommentsRequestDto dto, string userId);
        Task<CommentResponseDto> UpdateCommentAsync(long commentsId, UpdateCommentRequestDto dto);
        Task DeleteCommentAsync(long commentId);
        Task<List<CommentResponseDto>> GetCommentsByMovieId(long movieId);
    }
}