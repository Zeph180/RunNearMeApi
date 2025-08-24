using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request.Challenge;

public class UpdateChallengeRequest
{
        public Guid RunnerId { get; set; }
        public Guid ChallengeId { get; set; }
        [MaxLength(50)]
        public required string Name { get; set; }
        [MaxLength(10)]
        public required string Target { get; set; }
        [MaxLength(50)]
        public required string Description { get; set; }
        public DateTime EndsAt { get; set; }
}