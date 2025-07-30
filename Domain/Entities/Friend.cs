using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Friend
{
    public Guid FriendId { get; set; }
    public Guid RequestFrom { get; set; }
    public Guid RequestTo { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [MaxLength(1)]
    public required string Status { get; set; }
    public bool IsDeleted { get; set; } = false;
    public ICollection<Profile>? Profiles { get; set; }
}