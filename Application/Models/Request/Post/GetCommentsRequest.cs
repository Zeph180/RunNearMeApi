namespace Application.Models.Request.Posts;

public class GetCommentsRequest : HasPagination
{
    public Guid PostId { get; set; }
}