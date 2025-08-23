using Application.Interfaces.Dtos.Challenge;
using Application.Models.Request.Challenge;

namespace Application.Interfaces;

public interface IChallengeRepository
{
    Task<ChallengeDto> CreateChallenge(CreateChallengeRequest request);
    Task<bool> DeleteChallenge(ChallengeJoinRequest request);
    Task<List<ChallengeDto>> GetActiveChallenges(GetChallengesRequest request);
    Task<ChallengeDto> JoinChallenge(ChallengeJoinRequest request);
    Task<ChallengeDto> ExitChallenge(ChallengeJoinRequest request);
    
}