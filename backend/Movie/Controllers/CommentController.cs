using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Dtos.Comments.Request;
using Movie.Dtos.Comments.Response;
using Movie.Services.Interfaces;

namespace Movie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController: ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{movieId:long}")]
        public async Task<ActionResult<List<CommentResponseDto>>> GetComments([FromRoute] long movieId)
            => Ok(await _commentService.GetCommentsByMovieId(movieId));

        [Authorize]
        [HttpPost("{movieId:long}")]
        public async Task<IActionResult> AddCommentAsync(long movieId, [FromBody] CreateCommentsRequestDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("User ID not found in token.");

            var result = await _commentService.AddCommentAsync(dto, userId, movieId);

            return result == null ? NotFound() : Ok(result);
        }

        [Authorize]
        [HttpPut("{commentId:long}")]
        public async Task<IActionResult> UpdateCommentAsync(long commentId, [FromBody] UpdateCommentRequestDto dto)
        {
            var result = await _commentService.UpdateCommentAsync(commentId, dto);

            return result == null ? NotFound() : Ok(result);
        }

        [Authorize]
        [HttpDelete("{commentId:long}")]
        public async Task<IActionResult> DeleteCommentAsync(long commentId)
        {
            await _commentService.DeleteCommentAsync(commentId);
            return NoContent();
        }
        
    }
}