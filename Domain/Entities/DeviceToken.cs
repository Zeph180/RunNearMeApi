using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class DeviceToken
{
    public Guid Id { get; set; }
    [ForeignKey("RunnerId")]
    public Guid RunnerId { get; set; }
    public required string Token { get; set; }
    public required string Platform { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}