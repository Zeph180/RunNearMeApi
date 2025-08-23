using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Challenge
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
    public DateTime CreatedAt { get; set; }
    public DateTime EndsAt { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<Profile>? Challengers { get; set; }
}