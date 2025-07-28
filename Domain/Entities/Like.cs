namespace Domain.Entities;

public class Like
{
    public Guid LikeId { get; set; }
    public Guid RunnerId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public Guid? PostId { get; set; }
    public Post? Post { get; set; }

    public Guid? CommentId { get; set; }
    public Comment? Comment { get; set; }
}