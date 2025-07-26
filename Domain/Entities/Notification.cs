namespace Domain.Entities;

public class Notification
{
    public Guid NotificationId { get; set; }
    public Guid RunnerId { get; set; }
    public required string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; } = false;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}