namespace Application.Models.Request;

public class HasLocation
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double RadiusInMeters { get; set; }
}