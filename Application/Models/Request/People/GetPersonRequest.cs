namespace Application.Models.Request.People;

public class GetPersonRequest
{
    public required Guid RunnerId { get; set; }
    public required Guid PersonId { get; set; }
}