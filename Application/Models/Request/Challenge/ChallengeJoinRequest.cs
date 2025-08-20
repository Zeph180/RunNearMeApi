namespace Application.Models.Request.Challenge;

public class ChallengeJoinRequest
{
    public Guid ChallengeId { get; set; }
    public Guid RunnerId { get; set; }
}