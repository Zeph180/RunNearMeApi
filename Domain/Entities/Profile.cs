using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Profile
{
    [Key, ForeignKey("Runner")] public Guid RunnerId { get; set; }
    [Required] [MaxLength(10)] public string NickName { get; set; }
    [Required] [MaxLength(13)] [Phone] public required string PhoneNumber { get; set; }
    [Required] [MaxLength(50)] public required string Address { get; set; }
    [Required]
     [MaxLength(20)] public required string City { get; set; }
    [Required]
    [MaxLength(20)]public required string State { get; set; }
    [Range(14, 100)] public int Age { get; set; }
    [Range(100, 250)] public int Height { get; set; }
    [Range(20, 300)] public int Weight { get; set; }
    public DateTime? CompletedAt { get; set; }

    public virtual Runner? Runner { get; set; }
    public virtual ICollection<Run> Runs { get; set; } = new List<Run>();
    public virtual ICollection<Notification> Notifications { get; set; }  = new List<Notification>();
    public virtual ICollection<Friend>? Friends { get; set; } = new List<Friend>();
    public virtual ICollection<Post>? Posts { get; set; } =  new List<Post>();
    public virtual ICollection<Challenge>? Challenges { get; set; } = new List<Challenge>();
    public virtual ICollection<Group>? Groups { get; set; } = new List<Group>();
}