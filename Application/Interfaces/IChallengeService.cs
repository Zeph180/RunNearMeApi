using Application.Interfaces.Dtos.Challenge;
using Application.Models.Request.Challenge;

namespace Application.Interfaces;

public interface IChallengeService
{
    Task<ChallengeDto> CreateChallenge(CreateChallengeRequest request);
    Task<ChallengeDto> DeleteChallenge(CreateChallengeRequest request);
    Task<List<ChallengeDto>> GetChallenges();
    Task<ChallengeDto> JoinChallenge(ChallengeJoinRequest request);
    Task<ChallengeDto> ExitChallenge(ChallengeJoinRequest request);
    
}