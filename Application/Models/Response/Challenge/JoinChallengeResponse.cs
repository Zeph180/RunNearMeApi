using Application.Interfaces.Dtos.Challenge;

namespace Application.Models.Response.Challenge;

public class JoinChallengeResponse
{
    public ChallengeDto? Challenge { get; set; }
    public required string Status { get; set; }
}