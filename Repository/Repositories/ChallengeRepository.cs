using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Models.Request.Challenges;
using Application.Models.Response.Challenges;

namespace Repository.Repositories
{
    public  class ChallengeRepository : IChallenge
    {
        public Task<CreateChallengesResponse> CreateChallenge(CreateChallengeRequest createGroupRequest)
        {
            // Implementation of group creation logic goes here
            throw new NotImplementedException();
        }

    }
}
