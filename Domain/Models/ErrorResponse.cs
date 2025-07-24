namespace Domain.Models;

public class ErrorResponse
{
    public required string Message { get; set; }
    public required string ErrorCode { get; set; }
    public int StatusCode { get; set; }
    public required string TraceId { get; set; }
    public DateTime TimeStamp { get; set; }
    public Dictionary<string, object>? Details { get; set; }
}