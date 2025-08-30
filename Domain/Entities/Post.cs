using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Post
{
    public Guid PostId { get; set; } = Guid.NewGuid();
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
    [ForeignKey("RunnerId")]
    public Guid RunnerId { get; set; }
    public required Profile Poster { get; set; }
}