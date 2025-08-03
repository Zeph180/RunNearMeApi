namespace Domain.Entities;

public class RunRoutePoints
{
    public Guid RoutePointId { get; set; }
    public Decimal Latitude { get; set; }
    public Decimal Longitude { get; set; }
    public DateTime Timestamp { get; set; }
    
    //Navigational properties
    public Guid RunId { get; set; }
    public Guid RunnerId { get; set; }
    public required Runner Runner { get; set; }
    public required Run Run { get; set; }
}