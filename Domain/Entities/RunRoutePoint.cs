using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Domain.Entities;

public class RunRoutePoint
{
    [Key]
    public Guid RunRoutePointId { get; set; }
    [ForeignKey(nameof(Run))]
    public Guid RunId { get; set; }
    
    //NTS Points for efficient spatial operations
    public Point Location { get; set; } = null!;
    
    [Range(0, double.MaxValue)]
    public double? Altitude { get; set; }
    
    public DateTime Timestamp { get; set; }
    
    [Range(0, double.MaxValue)]
    public int SequenceNumber { get; set; }


    [Range(-90, 90)] public Decimal Latitude => (decimal)Location.Y;
    [Range(-180, 180)]
    public Decimal Longitude => (decimal)Location.X;
    
    public virtual Run Run { get; set; } = null!;
}