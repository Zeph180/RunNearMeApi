namespace Application.Models.Response.People;

public class GetPersonResponse
{
    public Guid RunnerId { get; set; }
    public required string NickName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
}