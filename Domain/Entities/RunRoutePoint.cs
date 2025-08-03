using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class RunRoutePoint
{
    public Guid RunRoutePointId { get; set; }
    public Decimal Latitude { get; set; }
    public Decimal Longitude { get; set; }
    public DateTime Timestamp { get; set; }
}