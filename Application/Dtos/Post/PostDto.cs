using Application.Models.Response.People;
using Domain.Entities;

namespace Application.Interfaces.Dtos.Post;

public class PostDto
{
    public Guid PostId { get; set; }
    public string? Caption { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ImageUrl { get; set; }
    public string? VideoUrl { get; set; }
    public string? Location { get; set; }
    public required Person Poster { get; set; }
    public int LikesCount { get; set; }
    public ICollection<CommentDto>? Comments { get; set; }
}
