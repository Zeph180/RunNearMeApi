namespace Application.Models.Response;

public class PushNotitficationResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public List<string>? FailedTokens { get; set; }
}