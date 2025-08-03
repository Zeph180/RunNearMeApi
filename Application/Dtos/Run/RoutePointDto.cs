namespace Application.Interfaces.Dtos.Run;

public class RoutePointDto
{
    public Decimal Latitude { get; set; }
    public Decimal Longitude { get; set; }
    public double? Altitude { get; set; }
    public DateTime Timestamp { get; set; }
    public int SequenceNumber { get; set; }
}