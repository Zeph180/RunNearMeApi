namespace Application.Models.Request.Posts;

public class CommentRequest
{
    public Guid RunnerId { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid PostId { get; set; }
}