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

    [HttpPost("register-device-token")]
    public async Task <IActionResult> RegisterDeviceToken([FromBody] RegisterDeviceTokenRequest request)
    {
        var response = await _deviceTokenService.SaveDeviceTokenAsync(request);
        return Ok(ApiResponse<object>.SuccessResponse(response));
    }

    [HttpPost("send-push-notification")]
    public async Task<IActionResult> SendSinglePushNotification([FromBody] PushNotificationRequest request)
    {
        var response = await _pushNotificationService.SendNotificationAsync(request);
        return Ok(ApiResponse<object>.SuccessResponse(response));
    }

    public async Task<IActionResult> SendPushNotificationToUser([FromBody] PushNotificationToUserRequest request)
    {
        var response = _pushNotificationService.SendNotificationToUserAsync(request)
    }
}