using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Group
{
    public Guid GroupId { get; set; }
    public Guid RunnerId { get; set; }
    [MaxLength(100)]
    public required string Name { get; set; }
    [MaxLength(100)]
    public required string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public ICollection<Profile>? Profiles { get; set; }
}