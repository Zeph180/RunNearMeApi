namespace Application.Models.Request.Posts;

public class GetPostsRequest : HasPagination
{
    public bool IsAdmin { get; set; } = false;
}