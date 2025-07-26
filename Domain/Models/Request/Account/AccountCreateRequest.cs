using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Request.Runner;

public class AccountCreateRequest
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
}