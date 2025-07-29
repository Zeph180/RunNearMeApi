namespace Application.Models.Response.People;

public class FriendRequestResponse
{
    public Guid RequestId { get; set; }
    public string RequestStatus { get; set; } = "Pending";
    public required string NickName { get; set; }
    public required string Address { get; set; }
}