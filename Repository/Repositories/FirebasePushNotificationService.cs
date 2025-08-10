using Application.Interfaces;
using Application.Interfaces.Dtos.PushNotifications;
using Application.Models.Request.PushNotification;
using Application.Models.Response;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Repository.Repositories;

public class FirebasePushNotificationService : IPushNotificationService
{
    private readonly FirebaseMessaging _firebaseMessaging;
    private readonly ILogger<FirebasePushNotificationService> _logger;
    private readonly IDeviceTokenService _deviceTokenService;

    public FirebasePushNotificationService( IOptions<FirebaseConfig> _firebaseConfig,
        ILogger<FirebasePushNotificationService> logger, IDeviceTokenService deviceTokenService)
    {
        _logger = logger;
        _deviceTokenService = deviceTokenService;

        try
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                var credentials = GoogleCredential.FromFile(_firebaseConfig.Value.ServiceAccountKeyPath);
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = credentials,
                });
            }

            _firebaseMessaging = FirebaseMessaging.DefaultInstance;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to initialise firebase admin sdk");
            throw;
        }
    }

    public async Task<PushNotitficationResponse> SendNotificationAsync(PushNotificationRequest request)
    {
        var messages = new List<Message>();

        foreach (var token in request.DeviceTokens)
        {
            var message = new Message()
            {
                Token = token,
                Notification = new Notification()
                {
                    Title = request.Title,
                    Body = request.Body,
                    ImageUrl = request.ImageUrl,
                },
                Data = request.Data,
                Android = new AndroidConfig()
                {
                    Notification = new AndroidNotification()
                    {
                        ClickAction = request.ClickAction,
                        ChannelId = "default_channel"
                    }
                },
                Apns = new ApnsConfig()
                {
                    Aps = new Aps()
                    {
                        Category = request.ClickAction
                    }
                }
            };
            messages.Add(message);
        }
        
        var batchResponse = await _firebaseMessaging.SendEachAsync(messages);
        var failedTokens = new List<string>();
        for (int i = 0; i < batchResponse.Responses.Count; i++)
        {
            if (!batchResponse.Responses[i].IsSuccess)
            {
                failedTokens.Add(request.DeviceTokens[i]);
                _logger.LogWarning($"Failed to send push notification to {request.DeviceTokens[i]}: {batchResponse.Responses[i].Exception}");
            }
        }

        if (failedTokens.Any())
        {
            await _deviceTokenService.RemoveInvalidTokensAsync(failedTokens);
        }

        return new PushNotitficationResponse()
        {
            Success = batchResponse.Responses.Count > 0,
            Message = $"Successfully sent {batchResponse.SuccessCount} push notifications",
            SuccessCount = batchResponse.SuccessCount,
            FailureCount = batchResponse.FailureCount,
            FailedTokens = failedTokens,
        };
    }

    public async Task<PushNotitficationResponse> SendNotificationToUserAsync(PushNotificationToUserRequest userRequest)
    {
        var deviceTokens = await _deviceTokenService.GetUserDeviceTokensAsync(userRequest.UserId);;
        if (deviceTokens.Count < 0)
        {
            _logger.LogInformation("Device token ID not found for {UserId}", userRequest.UserId);
            throw new KeyNotFoundException($"Device token ID not found for {userRequest.UserId}");
        }

        var request = new PushNotificationRequest
        {
            Title = userRequest.Title,
            Body = userRequest.Body,
            DeviceTokens = deviceTokens,
            Data = userRequest.Data,
        };
        return await SendNotificationAsync(request);
    }

    public async Task<PushNotitficationResponse> SendNotificationToTopicAsync(string topic, string title, string body, Dictionary<string, string>? data = null)
    {
        var message = new Message()
        {
            Topic = topic,
            Notification = new Notification()
            {
                Title = title,
                Body = body
            },
            Data = data,
            Android = new AndroidConfig()
            {
                Notification = new AndroidNotification()
                {
                    ChannelId = "default_channel"
                }
            }
        };

        var response = await _firebaseMessaging.SendAsync(message);
        return new PushNotitficationResponse
        {
            Success = true,
            Message = $"Push notification sent to topic {topic}. Message ID: {response}",
            SuccessCount = 1,
            FailureCount = 0,
        };
    }

    public async Task<bool> SubscribeToTopicAsync(List<string> deviceToken, string topic)
    {
        var response = await _firebaseMessaging.SubscribeToTopicAsync(deviceToken, topic);
        _logger.LogInformation($"Successfully subscribed {response.SuccessCount} to topic {topic}");
        return response.SuccessCount > 0;
    }

    public async Task<bool> UnSubscribeToTopicAsync(List<string> deviceToken, string topic)
    {
        var response = await _firebaseMessaging.UnsubscribeFromTopicAsync(deviceToken, topic);
        _logger.LogInformation($"Successfully subscribed {deviceToken} to topic {topic}");
        return response.SuccessCount > 0;
    }
}