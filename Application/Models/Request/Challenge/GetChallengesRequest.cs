namespace Application.Models.Request.Challenge;

public class GetChallengesRequest : HasPagination
{
        public  bool Joined { get; set; }
}