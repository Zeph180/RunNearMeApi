using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class RunRoutePoint
{
    [Key]
    public Guid RunRoutePointId { get; set; }
    [ForeignKey(nameof(Run))]
    public Guid RunId { get; set; }
    
    [Range(-90, 90)]
    public Decimal Latitude { get; set; }
    [Range(-180, 180)]
    public Decimal Longitude { get; set; }
    
    [Range(0, double.MaxValue)]
    public double? Altitude { get; set; }
    
    public DateTime Timestamp { get; set; }
    
    [Range(0, double.MaxValue)]
    public int SequenceNumber { get; set; }
    
    public virtual Run Run { get; set; } = null!;
}