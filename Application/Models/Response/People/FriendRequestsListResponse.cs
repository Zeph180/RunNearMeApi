namespace Application.Models.Response.People;

public class FriendRequestsListResponse
{
    public ICollection<FriendRequestResponse>? SentRequests  { get; set; }
    public ICollection<FriendRequestResponse>? ReceivedRequests { get; set; }
}