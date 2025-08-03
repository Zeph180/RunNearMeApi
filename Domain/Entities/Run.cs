using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Run
{
    public Guid RunId { get; set; }
    [ForeignKey("RunnerId")]
    public Guid RunnerId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double DistanceInMeters { get; set; }
    public double DurationInSeconds { get; set; }
    public double AveragePace { get; set; }
    public double MaxPace { get; set; }
    public double CaloriesBurned { get; set; }
    public double AverageHeartRate { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public required Profile Profile { get; set; }
    public required List<RunRoutePoint> RoutePoints { get; set; }
}