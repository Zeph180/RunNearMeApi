namespace Application.Models.Response.Run;

public class CompleteRunResponse
{
    public Guid RunId { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}