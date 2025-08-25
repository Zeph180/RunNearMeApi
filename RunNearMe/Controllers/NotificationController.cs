using Application.Interfaces;
using Application.Interfaces.Dtos.PushNotifications;
using Application.Models.Request.PushNotification;
using Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RunNearMe.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly IPushNotificationService _pushNotificationService;
    private readonly IDeviceTokenService _deviceTokenService;

    public NotificationController(INotificationService notificationService, IPushNotificationService pushNotificationService, IDeviceTokenService deviceTokenService)
    {
        _notificationService = notificationService;
        _pushNotificationService = pushNotificationService;
        _deviceTokenService = deviceTokenService;
    }

    /// <summary>
    /// This saves the client device fcm token
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("register-device-token")]
    public async Task <IActionResult> RegisterDeviceToken([FromBody] RegisterDeviceTokenRequest request)
    {
        var response = await _deviceTokenService.SaveDeviceTokenAsync(request);
        return Ok(ApiResponse<object>.SuccessResponse(response));
    }

    /// <summary>
    /// Sends a single push notification to a single client device
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("send-push-notification")]
    public async Task<IActionResult> SendSinglePushNotification([FromBody] PushNotificationRequest request)
    {
        var response = await _pushNotificationService.SendNotificationAsync(request);
        return Ok(ApiResponse<object>.SuccessResponse(response));
    }

    
    [HttpPost("send-push-notification-to-user")]
    public async Task<IActionResult> SendPushNotificationToUser([FromBody] PushNotificationToUserRequest request)
    {
        var response = await _pushNotificationService.SendNotificationToUserAsync(request);
        return Ok(ApiResponse<object>.SuccessResponse(response));
    }
    
    /// <summary>
    /// Used to subscribe to topic. This enables push broadcasting 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <example>A user automatically subscribes to the topic of a challenge they join</example>
    [HttpPost("subscribe-to-topic")]
    public async Task<IActionResult> SubscribeToTopic([FromBody] SubscribeToTopicRequest request)
    {
        var response = await _pushNotificationService.SubscribeToTopicAsync(request);
        return Ok(ApiResponse<object>.SuccessResponse(response));
    }
}