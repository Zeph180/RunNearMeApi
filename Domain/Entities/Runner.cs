using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Runner
{
    public required Guid RunnerId { get; set; }
    [MaxLength(50)]
    public required string Name { get; set; }
    [MaxLength(20)]
    public required string Password { get; set; }
    [MaxLength(50)]
    [EmailAddress]
    public required string Email { get; set; }
    public Profile? Profile { get; set; }
    public bool EmailConfirmed { get; set; }
    [MaxLength(100)]
    public string? EmailConfirmationToken { get; set; }
    public DateTime TokenGeneratedAt { get; set; }
    public DateTime TokenConfirmedAt { get; set; }
}