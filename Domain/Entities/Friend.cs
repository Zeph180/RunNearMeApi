using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Friend
{
    public Guid FriendId { get; set; }
    
    public Guid RequestFrom { get; set; }
    public Guid RequestTo { get; set; }
    
    [ForeignKey("RequestFrom")]
    public required Profile RequestFromProfile { get; set; }
    [ForeignKey("RequestTo")]
    public required Profile RequestToProfile { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    [MaxLength(1)]
    public required string Status { get; set; }
    public bool IsDeleted { get; set; } = false;
}