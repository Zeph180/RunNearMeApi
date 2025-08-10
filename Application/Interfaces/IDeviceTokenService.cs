using Application.Interfaces.Dtos.PushNotifications;

namespace Application.Interfaces;

public interface IDeviceTokenService
{
    Task<bool> SaveDeviceTokenAsync(RegisterDeviceTokenRequest request);
    Task<List<string>> GetUserDeviceTokensAsync(Guid userId);
    Task<bool> RemoveDeviceTokenAsync(Guid userId, string deviceToken);
    Task<bool> RemoveInvalidTokensAsync(List<string> invalidTokens);
    Task<bool> UpdateDeviceTokenAsync(Guid userId, string oldToken, string newToken);
}