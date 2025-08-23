using Application.Errors;
using Application.Interfaces;
using Application.Interfaces.Dtos.PushNotifications;
using Application.Middlewares.ErrorHandling;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Persistence;

namespace Repository.Repositories;

public class DeviceTokenService : IDeviceTokenService
{
    private readonly AppDbContext  _dbContext;
    private readonly ILogger<DeviceTokenService> _logger;

    public DeviceTokenService(AppDbContext dbContext, ILogger<DeviceTokenService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<bool> SaveDeviceTokenAsync(RegisterDeviceTokenRequest request)
    {
        var existingToken = await _dbContext.DeviceTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Token == request.DeviceToken && d.Platform == request.Platform);

        if (existingToken != null)
        {
            _logger.LogInformation($"Token {existingToken.Token} already exists for platform {request.Platform}");
            throw new BusinessException(ErrorCodes.ResourceAlreadyExists, ErrorMessages.ResourceAlreadyExists);
        }

        var deviceTokenEntity = new DeviceToken
        {
            RunnerId = request.UserId,
            Token = request.DeviceToken,
            Platform = request.Platform,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };
        
        _dbContext.DeviceTokens.Add(deviceTokenEntity);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<string>> GetUserDeviceTokensAsync(Guid userId)
    {
        try
        {
            return await _dbContext.DeviceTokens
                .AsNoTracking()
                .Where(dt => dt.RunnerId == userId && dt.IsActive)
                .Select(dt => dt.Token)
                .ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error getting user device tokens for user {userId}");
            throw;
        }
    }

    public async Task<bool> RemoveDeviceTokenAsync(Guid userId, string deviceToken)
    {
        try
        {
            var token = await _dbContext.DeviceTokens
                .FirstOrDefaultAsync(dt => dt.RunnerId == userId && dt.Token == deviceToken);
            if (token != null)
            {
                token.IsActive = false;
                token.UpdatedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error deleting user device token {deviceToken} for user {userId}");
            throw;
        }
    }

    public async Task<bool> RemoveInvalidTokensAsync(List<string> invalidTokens)
    {
        try
        {
            var tokens = await _dbContext.DeviceTokens
                .Where(dt => invalidTokens.Contains(dt.Token))
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.IsActive = false;
                token.UpdatedAt = DateTime.UtcNow;
            }
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Removed {tokens.Count} invalid device tokens");
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    public async Task<bool> UpdateDeviceTokenAsync(Guid userId, string oldToken, string newToken)
    {
        try
        {
            var token = await _dbContext.DeviceTokens
                .FirstOrDefaultAsync(dt => dt.Token == oldToken && dt.RunnerId == userId);

            if (token != null)
            {
                _logger.LogInformation($"Token {token.Token} exists for user {userId}");
                token.Token = newToken;
                token.UpdatedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            _logger.LogInformation($"Token {oldToken} does not exist for user {userId}");
            return false;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error updating device token {oldToken} for user {userId}");
            throw;
        }
    }
}