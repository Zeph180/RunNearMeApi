namespace Application.Models.Request.Challenge;

public class GetChallengesRequest
{
        public required Guid RunnerId { get; set; }
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; }
        public  bool Joined { get; set; }
}