using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Post
{
    public Guid PostId { get; set; }
    public Guid RunnerId { get; set; }
    [MaxLength(500)]
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    [MaxLength(100)]
    [Url]
    public string? ImageUrl { get; set; }
    [MaxLength(100)]
    [Url]
    public string? VideoUrl { get; set; }
    [MaxLength(100)]
    public string? Location { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<Like>? Likes { get; set; }
}