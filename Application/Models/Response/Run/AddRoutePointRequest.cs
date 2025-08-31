namespace Application.Models.Response.Run;

public class AddRoutePointRequest
{
    public Guid RunId { get; set; }
    public Guid RunnerId { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public double? Altitude { get; set; }
    public DateTime? Timestamp { get; set; }
}