using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Models.Response.Challenges
{
    public class JoinChallengeResp
    {
        public Guid ChallengeId { get; set; }
        public Guid RunnerId { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
