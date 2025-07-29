using Application.Enums;
using Application.Interfaces;
using Application.Middlewares.ErrorHandling;
using Application.Models.Request.People;
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
        var requester = await _dbContext.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.RunnerId == request.RunnerId);
        if (requester == null)
        {
            throw new BusinessException(
                "User not allowed to access this resource.",
                "USER_NOT_ALLOWED",
                401);
        }
        
        var person = await _dbContext.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.RunnerId == request.PersonId) ;
        if (person == null)
        {
            throw new BusinessException(
                "Person not found",
                "PERSON_NOT_FOUND",
                 404);
        }

        return _mapper.Map<Profile, GetPersonResponse>(person);
    }

    public async Task<FriendRequestResponse> SendFriendRequest(GetPersonRequest request)
    {
        var requester = await _dbContext.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.RunnerId == request.RunnerId);
        if (requester == null)
        {
            throw new BusinessException(
                "You are not allowed to access this resource.",
                "USER_NOT_ALLOWED",
                401);
        }
        
        var person = await _dbContext.Profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.RunnerId == request.PersonId) ;

        if (person == null)
        {
            throw new BusinessException(
                "Person not found",
                "PERSON_NOT_FOUND",
                404);
        }

        var existingRequest = _dbContext.Friends
            .AsNoTracking()
            .Where(r => r.RequestFrom == requester.RunnerId && r.RequestTo == person.RunnerId);
        if (existingRequest != null){
            throw new BusinessException(
                "Friend request already exists",
                "DUPLICATE_REQUEST",
                409);
        }

        var friendRequest = _mapper.Map<Profile, Friend>(person);
        friendRequest.RequestFrom = request.RunnerId;
        friendRequest.Status = "P";
        var response = await _dbContext.AddAsync(friendRequest);
        var result = await _dbContext.SaveChangesAsync();

        //Email sending can be queued
        //_emailService.SendAsync("zephrichards1@gmail.com", "from", "htmlMessage");
        return _mapper.Map<Friend, FriendRequestResponse>(friendRequest);
    }
}