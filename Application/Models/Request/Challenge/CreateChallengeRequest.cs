using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request.Challenge;

public class CreateChallengeRequest
{
    public Guid RunnerId { get; set; }
    [MaxLength(50)]
    public required string Name { get; set; }
    [MaxLength(10)]
    public required string Target { get; set; }
    [MaxLength(500)]
    public required string Description { get; set; }
    [MaxLength(100)]
    [Url]
    public required string ImageUrl { get; set; }
    public DateTime EndsAt { get; set; }
}