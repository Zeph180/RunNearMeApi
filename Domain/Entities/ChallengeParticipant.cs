using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Base;

namespace Domain.Entities;

public class ChallengeParticipant : BaseEntity
{
    [Key]
    public Guid ChallengeParticipantId { get; set; }
    
    [ForeignKey(nameof(Challenge))]
    public Guid ChallengeId { get; set; }
    
    [ForeignKey(nameof(Participant))]
    public Guid RunnerId { get; set; }
    
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public bool IsCompleted { get; set; } = false;
    public DateTime? CompletedAt { get; set; }
    
    public virtual Challenge? Challenge { get; set; }
    public virtual Profile? Participant { get; set; }
}