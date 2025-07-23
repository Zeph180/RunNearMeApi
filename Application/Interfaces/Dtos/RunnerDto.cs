namespace Application.Interfaces.Dtos;

public class RunnerDto
{
    public required string Name { get; set; }
    public required string NickName { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public int Age { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
}