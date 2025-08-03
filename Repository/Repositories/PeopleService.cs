using Application.Enums;
using Application.Errors;
using Application.Interfaces;
using Application.Middlewares.ErrorHandling;
using Application.Models.Request.People;
using Application.Models.Response;
using Application.Models.Response.People;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.Persistence;
using Profile = Domain.Entities.Profile;

namespace Repository.Repositories;

public class PeopleService : IPeople
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;

    public PeopleService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration,
        IEmailService emailService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
        _emailService = emailService;
    }

    public async Task<List<Person>> GetPeople(Guid runnerId, int pageNumber = 1, int pageSize = 10)
    {
        var requester = await GetValidProfileAsync(runnerId, "USER_NOT_ALLOWED",
            "You are not allowed to access this resource.");

        var people = await _dbContext.Profiles
            .Where(p => p.RunnerId != runnerId)
            .OrderBy(p => p.NickName)
            .Skip((pageNumber - 1) * pageSize)
            .Select(p => new Person
            {
                RunnerId = p.RunnerId,
                NickName = p.NickName
            }).ToListAsync();
        return people;
    }

    public async Task<GetPersonResponse> GetPerson(GetPersonRequest request)
    {
        var requester = await GetValidProfileAsync(request.RequesterId, "USER_NOT_ALLOWED",
            "You are not allowed to access this resource.");
        var person = await GetValidProfileAsync(request.RequestedId, "PERSON_NOT_FOUND", "Person not found.");

        return _mapper.Map<Profile, GetPersonResponse>(person);
    }

    public async Task<FriendRequestResponse> SendFriendRequest(GetPersonRequest request)
    {
        var requester = await GetValidProfileAsync(request.RequesterId, "USER_NOT_ALLOWED",
            "You are not allowed to access this resource.");
        var person = await GetValidProfileAsync(request.RequestedId, "PERSON_NOT_FOUND", "Person not found");

        var existingRequest = await _dbContext.Friends
            .AsNoTracking()
            .FirstOrDefaultAsync(r =>
                (r.RequestFrom == requester.RunnerId && r.RequestTo == person.RunnerId) ||
                (r.RequestFrom == person.RunnerId && r.RequestTo == requester.RunnerId)
            );
        if (existingRequest != null)
        {
            throw new BusinessException(
                "Friend request already exists",
                "DUPLICATE_REQUEST",
                409);
        }

        var friendRequest = _mapper.Map<Profile, Friend>(person);
        friendRequest.RequestFrom = request.RequesterId;
        friendRequest.Status = "P";
        await _dbContext.AddAsync(friendRequest);
        await _dbContext.SaveChangesAsync();

        //Email sending can be queued
        //_emailService.SendAsync("zephrichards1@gmail.com", "from", "htmlMessage");
        var response = _mapper.Map<Friend, FriendRequestResponse>(friendRequest);
        return response;
    }

    public async Task<FriendRequestResponse> GetFriendRequest(GetFriendRequestRequest request)
    {
        var requester = await GetValidProfileAsync(request.RequesterId, "USER_NOT_ALLOWED",
            "You are not allowed to access this resource.");
        var person = await GetValidProfileAsync(request.RequestedId, "PERSON_NOT_FOUND", "Person not found");

        var existingRequest =
            await GetExistingFriendRequestAsync(requester.RunnerId, person.RunnerId, request.FriendRequestId);
        if (existingRequest == null)
        {
            throw new BusinessException(
                "Friend request not found",
                "DUPLICATE_REQUEST",
                404);
        }

        return new FriendRequestResponse
        {
            FriendRequestId = request.FriendRequestId,
            RequesterId = request.RequesterId,
            NickName = person.NickName,
            Address = person.Address,
            RequestStatus = existingRequest.Status,
        };
    }

    public async Task<FriendRequestsListResponse> GetFriendRequests(Guid request, int pageSize = 10, int pageNumber = 1)
    {
        var requester = await GetValidProfileAsync(request, ErrorCodes.UserNotAllowed, ErrorMessages.UserNotAllowed);
        var allFriendRequests = await _dbContext.Friends
            .Include(fr => fr.RequestFromProfile)
            .Include(fr => fr.RequestToProfile)
            .Where(fr =>
                fr.Status == "P" && (fr.RequestTo == requester.RunnerId || fr.RequestFrom == requester.RunnerId))
            .Select(fr => fr)
            .ToListAsync();

        var sentRequests = allFriendRequests
            .Where(fr => fr.RequestFrom == requester.RunnerId)
            .Skip((pageNumber - 1) * pageSize)
            .Select(fr => new FriendRequestResponse
            {
                Address = fr.RequestFromProfile.Address,
                RequesterId = fr.RequestFromProfile.RunnerId,
                NickName = fr.RequestToProfile.NickName,
                FriendRequestId = fr.FriendId,
                RequestStatus = fr.Status,
            })
            .ToList();

        var receivedRequests = allFriendRequests
            .Where(fr => fr.RequestTo == requester.RunnerId)
            .Skip((pageNumber - 1) * pageSize)
            .Select(fr => new FriendRequestResponse
            {
                Address = fr.RequestFromProfile.Address,
                RequesterId = fr.RequestFromProfile.RunnerId,
                NickName = fr.RequestFromProfile.NickName,
                FriendRequestId = fr.FriendId,
                RequestStatus = fr.Status,
            })
            .ToList();

        var friendRequests = new FriendRequestsListResponse
        {
            ReceivedRequests = receivedRequests,
            SentRequests = sentRequests
        };
        return friendRequests;
    }

    public async Task<FriendRequestResponse> UpdateFriendRequest(UpdateFriendShip request)
    {
        var requester = await GetValidProfileAsync(request.CurrentUserId, ErrorCodes.UserNotAllowed,
            ErrorMessages.UserNotAllowed);
        var friendRequest =
            await GetExistingFriendRequestAsync(requester.RunnerId, request.RequestedId, request.FriendShipId, true);

        var isStatusValid = IsStatusValid(request.Status);

        if (request.Status == friendRequest.Status)
        {
            string statusMsg = request.Status switch
            {
                "A" => "ACCEPTED",
                "P" => "PENDING",
                "D" => "DECLINED",
                "I" => "IGNORED",
                "C" => "CANCELLED",
                _ => "UNKNOWN"
            };
            throw new BusinessException($"Friend is request already {statusMsg}");
        }

        friendRequest = await UpdateFriendRequestHelper(request);
        
        var response = _mapper.Map<Friend, FriendRequestResponse>(friendRequest);
        //_emailService.SendAsync("toemail", "new friend", "message");
        return response;
    }

    private async Task<Friend> UpdateFriendRequestHelper(UpdateFriendShip request)
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
    
    private async Task<Profile> GetValidProfileAsync(Guid runnerId, string errorCode, string errorMessage)
    {
        var profile = await _dbContext.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.RunnerId == runnerId);

        if (profile is null)
        {
            throw new BusinessException(errorMessage, errorCode, 404);
        }

        return profile;
    }

    private async Task<Friend?> GetExistingFriendRequestAsync(Guid currentUser, Guid requestedId,
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

    private Task<bool> IsStatusValid(string status)
    {
        var requestStatuses = _configuration.GetSection("Friendship:SentFriendRequestAllowedStatus")
            .Get<List<string>>();
        var isValid = requestStatuses.Contains(status);
        if (!isValid)
        {
            throw new BusinessException("Invalid friend request status", "INVALID_STATUS", 400);
        }

        return Task.FromResult(isValid);
    }
}