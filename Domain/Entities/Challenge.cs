namespace Domain.Entities;

public class Challenge
{
    public Guid ChallengeId { get; set; }
    public Guid RunnerId { get; set; }
    public required string Name { get; set; }
    public required string Target { get; set; }
    public required string Description { get; set; }
    public required string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public required Runner[] Runners { get; set; }
}