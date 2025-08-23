namespace Application.Models.Request.Challenge;

public class GetChallengesRequest
{
        public required Guid ChallengeId { get; set; }
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; }
        public  bool Joined { get; set; }
}