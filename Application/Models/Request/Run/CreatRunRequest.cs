namespace Application.Models.Request.Run;

public class CreatRunRequest
{
    public Guid RunnerId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}