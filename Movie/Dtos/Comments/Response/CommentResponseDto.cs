using Movie.Dtos.Auth;

namespace Movie.Dtos.Comments.Response
{
    public class CommentResponseDto
    {
        public long Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public ProfileResponseDto? User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
