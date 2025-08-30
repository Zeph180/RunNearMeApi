namespace Application.Models.Request.Posts;

public class GetPostRequest
{
    public Guid PostId { get; set; }
    public Guid RunnerId { get; set; }
}