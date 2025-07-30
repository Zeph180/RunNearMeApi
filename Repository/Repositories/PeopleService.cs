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

    public PeopleService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration, IEmailService emailService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
        _emailService = emailService;
    }
    
    

    public async Task<List<Person>> GetPeople(Guid runnerId)
    {
        if (!_dbContext.Profiles.Any(r => r.RunnerId == runnerId))
        {
            throw new BusinessException(
                "Either runner not found or profile incomplete.",
                "RUNNER_NOT_FOUND",
                404);
        }

        var people = await _dbContext.Profiles
            .Where(p => p.RunnerId != runnerId)
            .Select(p => new Person
            {
                RunnerId = p.RunnerId,
                NickName = p.NickName
            }).ToListAsync();
        return people;
    }

    public async Task<GetPersonResponse> GetPerson(GetPersonRequest request)
    {
        var requester = await GetValidProfileAsync(request.RequesterId, "USER_NOT_ALLOWED", "You are not allowed to access this resource.");
        var person = await GetValidProfileAsync(request.RequestedId, "PERSON_NOT_FOUND", "Person not found.");

        return _mapper.Map<Profile, GetPersonResponse>(person);
    }

    public async Task<FriendRequestResponse> SendFriendRequest(GetPersonRequest request)
    {
        var requester = await GetValidProfileAsync(request.RequesterId, "USER_NOT_ALLOWED", "You are not allowed to access this resource.");
        var person = await GetValidProfileAsync(request.RequestedId, "PERSON_NOT_FOUND", "Person not found");

        var existingRequest = await _dbContext.Friends
            .AsNoTracking()
            .FirstOrDefaultAsync(r => 
                (r.RequestFrom == requester.RunnerId && r.RequestTo == person.RunnerId) ||
                (r.RequestFrom == person.RunnerId && r.RequestTo == requester.RunnerId)
                );
        if (existingRequest != null){
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
        var requester = await GetValidProfileAsync(request.RequesterId, "USER_NOT_ALLOWED", "You are not allowed to access this resource.");
        var person = await GetValidProfileAsync(request.RequestedId, "PERSON_NOT_FOUND", "Person not found");
        
        var existingRequest = await GetExistingFriendRequestAsync(requester.RunnerId, person.RunnerId, request.FriendRequestId);
        if (existingRequest == null){
            throw new BusinessException(
                "Friend request not found",
                "DUPLICATE_REQUEST",
                404);
        }

        return new FriendRequestResponse
        {
            FriendRequestId = request.FriendRequestId,
            NickName = person.NickName,
            Address = person.Address,
            RequestStatus = existingRequest.Status,
        };
    }
    
    
    public async Task<List<FriendRequestResponse>> GetFriendRequests(GetFriendRequestRequest request)
    {
        var requester = await GetValidProfileAsync(request.RequesterId, ErrorCodes.UserNotAllowed, ErrorMessages.UserNotAllowed);
        var friendRequests = await _dbContext.Friends
            .Where(fr => fr.Status == "P" && fr.RequestTo == requester.RunnerId)
            .Select(fr => new FriendRequestResponse
            {
                Address = "",
                NickName = "",
                FriendRequestId = fr.FriendId,
                RequestStatus = fr.Status,
            }).ToListAsync();

        return friendRequests;
    }
    
    private async Task<Profile> GetValidProfileAsync(Guid runnerId, string errorCode, string errorMessage)
    {
        var profile = await _dbContext.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.RunnerId == runnerId);

        if (profile == null)
        {
            throw new BusinessException(errorMessage, errorCode, 404);
        }

        return profile;
    }
    
    private async Task<Friend?> GetExistingFriendRequestAsync(Guid requesterId, Guid requestedId, Guid? friendRequestId = null)
    {
        return await _dbContext.Friends
            .AsNoTracking()
            .FirstOrDefaultAsync(r =>
                r.RequestFrom == requesterId &&
                r.RequestTo == requestedId &&
                (friendRequestId == null || r.FriendId == friendRequestId));
    }
    
}