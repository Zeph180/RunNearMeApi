using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Notification
{
    public Guid NotificationId { get; set; }
    [ForeignKey("Profile")]
    public Guid RunnerId { get; set; }
    [MaxLength(500)]
    public required string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Profile? Profile { get; set; }
}