using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Runner
{
    public required Guid RunnerId { get; set; }
    public required string Name { get; set; }
    public required string NickName { get; set; }
    public required string Password { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    [Phone]
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    [Range(14, 100)]
    public int Age { get; set; }
    [Range(10, 200)]
    public int Height { get; set; }
    [Range(10, 200)]
    public int Weight { get; set; }
    public Run[]? Runs { get; set; }
    public Notification[]? Notifications { get; set; }
    public Friend[]? Friends { get; set; }
    public Post[]? Posts { get; set; }
    public Challenge[]? Challenges { get; set; }
    public Group[]? Groups { get; set; }
}