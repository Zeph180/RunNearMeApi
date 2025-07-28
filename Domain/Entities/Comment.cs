using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Comment
{
    public Guid CommentId { get; set; }
    public Guid RunnerId { get; set; }
    [MaxLength(500)]
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Like[]? Likes { get; set; }
}