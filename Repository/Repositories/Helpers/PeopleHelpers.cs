using Application.Errors;
using Application.Interfaces;
using Application.Middlewares.ErrorHandling;
using Application.Models.Request.People;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository.Persistence;
using Profile = Domain.Entities.Profile;

namespace Repository.Repositories.Helpers;

public class PeopleHelpers : IPeopleHelper
{
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PeopleHelpers> _logger;

    public PeopleHelpers(AppDbContext dbContext, IConfiguration configuration, ILogger<PeopleHelpers> logger)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _logger = logger;
    }
    
    public async Task<Friend> UpdateFriendRequestHelper(UpdateFriendShip request)
    {
        var pendingRequest = await GetExistingFriendRequestAsync(
            request.CurrentUserId, request.RequestedId, request.FriendShipId, true);
        if (pendingRequest is null)
        {
            throw new BusinessException(ErrorMessages.FriendRequestNotFound, ErrorCodes.FriendRequestNotFound, 404);
        }

        if (pendingRequest.RequestTo != request.CurrentUserId)
        {
            var allowedSentStatuses = _configuration.GetSection("Friendship:SentFriendRequestAllowedStatus")
                .Get<List<string>>();
            if (!allowedSentStatuses.Contains(request.Status))
            {
                throw new BusinessException(ErrorMessages.ActionNotAllowed, ErrorCodes.ActionNotAllowed);
            }
        }

        pendingRequest.Status = request.Status;
        _dbContext.Update(pendingRequest);
        await _dbContext.SaveChangesAsync();
        return pendingRequest;
    }
    
    public async Task<Profile> GetValidProfileAsync(Guid runnerId, string errorCode, string errorMessage)
    {
        _logger.LogInformation("Start runner valid check {RunnerId}", runnerId);
        var profile = await _dbContext.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.RunnerId == runnerId);

        if (profile is null)
        {
            _logger.LogInformation("Runner not found {RunnerId}", runnerId);
            throw new BusinessException(errorMessage, errorCode, 404);
        }

        return profile;
    }

    public async Task<Friend?> GetExistingFriendRequestAsync(Guid currentUser, Guid requestedId,
        Guid? friendShipId = null, bool track = false)
    {
        Friend? friendRequest;

        switch (track)
        {
            case false:
                friendRequest = await _dbContext.Friends
                    .AsNoTracking()
                    .Include(r => r.RequestFromProfile)
                    .Include(r => r.RequestToProfile)
                    .FirstOrDefaultAsync(r =>
                        (r.RequestFrom == currentUser && r.RequestTo == requestedId) ||
                        (r.RequestFrom == requestedId && r.RequestTo == currentUser));
                break;
            case true:
                friendRequest = await _dbContext.Friends
                    .Include(r => r.RequestFromProfile)
                    .Include(r => r.RequestToProfile)
                    .FirstOrDefaultAsync(r =>
                        r.RequestFrom == requestedId &&
                        r.RequestTo == currentUser && r.FriendId == friendShipId);
                break;
        }

        if (friendRequest is null)
        {
            throw new BusinessException(ErrorCodes.FriendRequestNotFound, ErrorCodes.FriendRequestNotFound, 404);
        }

        return friendRequest;
    }

}