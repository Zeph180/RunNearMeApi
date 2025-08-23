namespace Application.Interfaces.Dtos.PushNotifications;

public class RegisterDeviceTokenRequest
{
        public required string DeviceToken { get; set; }
        public required string Platform { get; set; }
        public required Guid UserId { get; set; }
}