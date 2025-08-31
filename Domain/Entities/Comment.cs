using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    
    public Guid PostId { get; set; }
    public Post? Post { get; set; }
    public required Profile Runner { get; set; }
    
    public Guid? ParentCommentId { get; set; }
    [ForeignKey("ParentCommentId")]
    public Comment? ParentComment { get; set; }
    
    public ICollection<Comment>? Replies { get; set; }
}