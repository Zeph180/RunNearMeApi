namespace Domain.Models.Response;

public class CreateAccountResponse
{
    public Guid RunnerId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}