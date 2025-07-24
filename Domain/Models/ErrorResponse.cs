namespace Domain.Models;

public class ErrorResponse
{
    public string Message { get; set; }
    public string ErrorCode { get; set; }
    public string StatusCode { get; set; }
    public string TraceId { get; set; }
    public DateTime TimeStamp { get; set; }
    public Dictionary<string, string> Details { get; set; }
}