using Domain.Entities;

namespace Application.Interfaces.Dtos.Post;

public class CommentDto
{
    public Guid PostId { get; set; }
    public Guid CommentId { get; set; }
    public Guid RunnerId { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Like> Likes { get; set; }
} 