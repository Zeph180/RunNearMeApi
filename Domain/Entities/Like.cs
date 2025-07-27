namespace Domain.Entities;

public class Like
{
    public Guid LikeId { get; set; }
    public Guid RunnerId { get; set; }
    public DateTime CreatedAt { get; set; }
}