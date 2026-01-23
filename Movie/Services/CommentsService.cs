using AutoMapper;
using Movie.Dtos.Comments.Request;
using Movie.Dtos.Comments.Response;
using Movie.Entities;
using Movie.Repositories.Interfaces;
using Movie.Services.Interfaces;

namespace Movie.Services
{
  public class CommentsService : ICommentService
  {
    private readonly ICommentsRepository _commentsRepository;
    private readonly IMapper _mapper;

    public CommentsService(ICommentsRepository commentsRepository, IMapper mapper)
    {
      _commentsRepository = commentsRepository;
      _mapper = mapper;
    }

    public async Task<CommentResponseDto> AddCommentAsync(CreateCommentsRequestDto dto, string userId, long movieId)
    {
        var comment = _mapper.Map<Comment>(dto);

        comment.MovieId = movieId;
        comment.UserId = userId;
        comment.CreatedAt = DateTime.UtcNow;
        comment.UpdatedAt = DateTime.UtcNow;

        await _commentsRepository.CreateCommentAsync(comment);
        return _mapper.Map<CommentResponseDto>(comment);

    }

    public async Task DeleteCommentAsync(long commentId)
    {
        var comment = await _commentsRepository.GetCommentByIdAsync(commentId)
            ?? throw new KeyNotFoundException($"Comment with ID {commentId} not found.");

        await _commentsRepository.DeleteCommentAsync(comment);
    }

    public async Task<List<CommentResponseDto>> GetCommentsByMovieId(long movieId)
    {
        var comments =  await _commentsRepository.GetCommentsByMovieIdAsync(movieId);
        return _mapper.Map<List<CommentResponseDto>>(comments);
    }

    public async Task<CommentResponseDto> UpdateCommentAsync(long commentsId, UpdateCommentRequestDto dto)
    {
        var comment = await _commentsRepository.GetCommentByIdAsync(commentsId)
            ?? throw new KeyNotFoundException($"Comment with ID {commentsId} not found.");

        _mapper.Map(dto, comment);
        comment.UpdatedAt = DateTime.UtcNow;
        
        await _commentsRepository.UpdateCommentAsync(comment);
        return _mapper.Map<CommentResponseDto>(comment);
    }
  }
}