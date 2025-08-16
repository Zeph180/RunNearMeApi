using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ChallengeParticipant
    {
        public Guid ChallengeId { get; set; }
        public Challenge Challenge { get; set; }
        public Guid RunnerId { get; set; }
        public Runner Runner { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        // Optional: status, progress, completion %
        public bool Completed { get; set; } = false;
    }
}
