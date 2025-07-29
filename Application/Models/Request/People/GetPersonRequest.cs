namespace Application.Models.Request.People;

public class GetPersonRequest
{
    public required Guid RequesterId { get; set; }
    public required Guid RequestedId { get; set; }
}