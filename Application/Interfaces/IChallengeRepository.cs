using Application.Interfaces.Dtos.Challenge;
using Application.Models.Request.Challenge;
using Application.Models.Response.Challenge;

namespace Application.Interfaces;

public interface IChallengeRepository
{
    Task<ChallengeDto> CreateChallenge(CreateChallengeRequest request);
    Task<JoinChallengeResponse> UpdateChallengeDetails(UpdateChallengeRequest request);
    Task<JoinChallengeResponse> UpdateChallengeArt(UpdateChallangeArtRequest request);
    Task<bool> DeleteChallenge(ChallengeJoinRequest request);
    Task<List<ChallengeDto>> GetActiveChallenges(GetChallengesRequest request);
    Task<JoinChallengeResponse> JoinChallenge(ChallengeJoinRequest request);
    Task<JoinChallengeResponse> ExitChallenge(ChallengeJoinRequest request);
    
}