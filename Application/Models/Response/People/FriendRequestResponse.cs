namespace Application.Models.Response.People;

public class FriendRequestResponse
{
    public Guid FriendRequestId { get; set; }
    public Guid RequesterId { get; set; }
    public required string RequestStatus { get; set; }
    public required string NickName { get; set; }
    public required string Address { get; set; }
}
