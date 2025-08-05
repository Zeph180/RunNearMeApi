using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response.Challenges
{
    public class CreateChallengesResponse
    {
        public Guid GroupId { get; set; }
        public DateTime CreatedAt { get; set; }
        public  string Name { get; set; }
        public  string Description { get; set; }

    }
}
