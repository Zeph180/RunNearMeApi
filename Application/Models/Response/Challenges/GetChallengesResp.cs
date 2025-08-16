using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response.Challenges
{
    public class GetChallengesResp
    {
        public Guid ChallengeId { get; set; }
        public string Name { get; set; }
        public string Target { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
