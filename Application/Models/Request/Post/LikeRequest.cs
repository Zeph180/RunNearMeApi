namespace Application.Models.Request.Posts;

public class ReactRequest
{
    public Guid PostId { get; set; }
    public Guid RunnerId { get; set; }
    public bool IsLike { get; set; }
}