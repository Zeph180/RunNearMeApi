using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Request.Challenges;
using Application.Models.Response.Challenges;

namespace Application.Interfaces
{
    public interface IChallenge
    {
        Task<CreateChallengesResponse> CreateChallenge(CreateChallengeRequest createGroupRequest);
    }
}
