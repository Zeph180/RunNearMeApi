namespace Domain.Entities;

public class Friend
{
    public Guid FriendId { get; set; }
    public Guid RunnerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Accepted { get; set; } = false;
    public bool IsDeleted { get; set; } = false;
}