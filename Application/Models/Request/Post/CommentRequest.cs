namespace Application.Models.Request.Posts;

public class CommentRequest
{
    public Guid RunnerId { get; set; }
    public required string Message { get; set; }
    public Guid PostId { get; set; }
}