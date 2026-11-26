namespace Movie.Dtos.Comments;

public class CommentDto
{
    public long Id { get; set; }
    
    public string Content { get; set; } = null!;
    
    public string Username { get; set; } = null!;
    
    public string CreatedAt { get; set; } = null!;
}