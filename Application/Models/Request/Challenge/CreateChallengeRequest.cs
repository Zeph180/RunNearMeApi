using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Application.Models.Request.Challenge;

public class CreateChallengeRequest
{
    public Guid RunnerId { get; set; }
    [MaxLength(50)]
    public required string Name { get; set; }
    [MaxLength(10)]
    public required string Target { get; set; }
    [MaxLength(50)]
    public required string Description { get; set; }
    public DateTime EndsAt { get; set; }
    public required IFormFile ChallengeArt { get; set; }
}