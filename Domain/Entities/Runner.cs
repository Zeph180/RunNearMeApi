using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Runner
{
    public required Guid RunnerId { get; set; }
    public required string Name { get; set; }
    public required string Password { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    public Profile? Profile { get; set; }
}