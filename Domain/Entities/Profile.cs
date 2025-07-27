using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Profile
{
    [Key, ForeignKey("Runner")]
    public Guid RunnerId { get; set; }
    public required string NickName { get; set; }
    [Phone]
    public required string PhoneNumber { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    [Range(14, 100)]
    public int Age { get; set; }
    [Range(10, 200)]
    public int Height { get; set; }
    [Range(10, 200)]
    public int Weight { get; set; }
    public ICollection<Run>? Runs { get; set; }
    public ICollection<Notification>? Notifications { get; set; }
    public ICollection<Friend>? Friends { get; set; }
    public ICollection<Post>? Posts { get; set; }
    public ICollection<Challenge>? Challenges { get; set; }
    public ICollection<Group>? Groups { get; set; }
}