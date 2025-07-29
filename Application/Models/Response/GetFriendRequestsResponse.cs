namespace Application.Models.Response;

public class GetFriendRequestResponse
{
    public Guid RequestId { get; set; }
    public Guid RequesterId { get; set; }
    public required string NickName { get; set; }
    public required string Address { get; set; }
}