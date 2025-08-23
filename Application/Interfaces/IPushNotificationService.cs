using Application.Interfaces.Dtos.PushNotifications;
using Application.Models.Request.PushNotification;
using Application.Models.Response;

namespace Application.Interfaces;

public interface IPushNotificationService
{
    Task<PushNotitficationResponse> SendNotificationAsync(PushNotificationRequest request);
    Task<PushNotitficationResponse> SendNotificationToUserAsync(PushNotificationToUserRequest request );
    Task<PushNotitficationResponse> SendNotificationToTopicAsync(string topic, string title, string body, Dictionary<string, string>? data = null);
    Task<bool> SubscribeToTopicAsync(SubscribeToTopicRequest request);
    Task<bool> UnSubscribeToTopicAsync(List<string> deviceToken, string topic);
}