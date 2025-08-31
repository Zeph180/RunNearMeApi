namespace Application.Models.Request.Run;

public class NearByRunRequest:HasLocation
{
    public Guid RunnerId { get; set; }
}