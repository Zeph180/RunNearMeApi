namespace Domain.Entities;

public class Post
{
    public Guid PostId { get; set; }
    public Guid RunnerId { get; set; }
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? ImageUrl { get; set; }
    public string? VideoUrl { get; set; }
    public string? Location { get; set; }
    public Comment[]? Comments { get; set; }
    public Like[]? Likes { get; set; }
}