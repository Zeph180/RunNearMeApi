namespace Application.Interfaces.Dtos.Run;

public class RunDto
{
    public Guid RunId { get; set; }
    public Guid RunnerId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double DistanceInMeters { get; set; }
    public double DurationInSeconds { get; set; }
    public double AveragePace { get; set; }
    public double MaxPace { get; set; }
    public double CaloriesBurned { get; set; }
    public double AverageHeartRate { get; set; }
    public required ICollection<RoutePointDto> RoutePoints  { get; set; }
}