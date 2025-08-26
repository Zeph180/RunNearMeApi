using Domain.Entities;

namespace Application.Interfaces;

public class CreatePostResponse
{
    public Guid PostId { get; set; } = Guid.NewGuid();
    public Guid RunnerId { get; set; }
    public required string Caption { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? ImageUrl { get; set; }
    public string? VideoUrl { get; set; }
    public string? Location { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<Like>? Likes { get; set; }
}