namespace Application.Models.Request.Run;

public class CreateRunRequest
{
    public Guid RunnerId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}