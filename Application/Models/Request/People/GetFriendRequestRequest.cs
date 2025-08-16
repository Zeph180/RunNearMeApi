namespace Application.Models.Request.People;

public class GetFriendRequestRequest : GetPersonRequest
{
    public Guid FriendRequestId { get; set; }
}