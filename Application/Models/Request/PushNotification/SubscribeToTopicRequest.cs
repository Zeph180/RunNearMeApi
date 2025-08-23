namespace Application.Models.Request.PushNotification;

public class SubscribeToTopicRequest
{
    public required string Topic { get; set; }
    public Guid UserId { get; set; }
}