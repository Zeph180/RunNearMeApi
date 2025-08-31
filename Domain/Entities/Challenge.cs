using System.ComponentModel.DataAnnotations;
using Domain.Entities.Base;

namespace Domain.Entities;

public class Challenge : SoftDeletableEntity
{
    public Guid ChallengeId { get; set; }
    public Guid RunnerId { get; set; }
    [MaxLength(50)]
    public required string Name { get; set; }
    [MaxLength(10)]
    public required string Target { get; set; }
    [MaxLength(100)]
    public required string Description { get; set; }
    [MaxLength(300)]
    [Url]
    public required string ImageUrl { get; set; }
    [MaxLength(100)]
    public required string PushTopic { get; set; }
    public DateTime EndsAt { get; set; }
    
    public virtual required Profile Creator { get; set; }
    public ICollection<ChallengeParticipant>? Challengers { get; set; }
}