using Microsoft.AspNetCore.Http;

namespace Application.Models.Request.Challenge;

public class UpdateChallangeArtRequest
{
    public Guid RunnerId { get; set; }
    public Guid ChallengeId { get; set; }
    public required IFormFile ChallengeArt {get; set;}
}