namespace Application.Interfaces.Dtos.PushNotifications;

public class PushNotificationToUserRequest
{
    public Guid UserId { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }
    public Dictionary<string, string>? Data { get; set; }
}