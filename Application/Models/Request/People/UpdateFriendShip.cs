namespace Application.Models.Request.People;

public class UpdateFriendShip
{
    public required Guid CurrentUserId { get; set; }
    public required Guid RequestedId { get; set; }
    public required Guid FriendShipId { get; set; }
    public required String Status { get; set; } 
}