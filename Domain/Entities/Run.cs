using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Run
{
    [Key] public Guid RunId { get; set; }
    [ForeignKey(nameof(Profile))] public Guid RunnerId { get; set; }

    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public DateTime? EndTime { get; set; }

    [Range(0, double.MaxValue)] public double DistanceInMeters { get; set; }

    [Range(0, double.MaxValue)] public double DurationInSeconds { get; set; }

    [Range(0, double.MaxValue)] public double AveragePace { get; set; }

    [Range(0, double.MaxValue)] public double MaxPace { get; set; }

    [Range(0, double.MaxValue)] public double CaloriesBurned { get; set; }

    [Range(0, double.MaxValue)] public double AverageHeartRate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual Profile Profile { get; set; } = null!;
    public virtual List<RunRoutePoint> RoutePoints { get; set; } = new List<RunRoutePoint>();
}