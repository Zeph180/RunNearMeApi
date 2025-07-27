using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request.Authentication;

public class CompleteProfileReq
{
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
}