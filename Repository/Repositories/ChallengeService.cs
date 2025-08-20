using Application.Interfaces;
using Application.Interfaces.Dtos.Challenge;
using Application.Models.Request.Challenge;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Repository.Persistence;

namespace Repository.Repositories;

public class ChallengeService : IChallengeService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IPeopleHelper _peopleHelper;
    private readonly ILogger<IChallengeService> _logger;
    
    public ChallengeService(AppDbContext dbContext, IMapper mapper, IPeopleHelper peopleHelper, ILogger<IChallengeService> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _peopleHelper = peopleHelper;
        _logger = logger;
    }
    
    public async Task<ChallengeDto> CreateChallenge(CreateChallengeRequest request)
    {
        try
        {
            _logger.LogInformation("Starting to create challenge");
            await _peopleHelper.GetValidProfileAsync(request.RunnerId, "USER_NOT_ALLOWED",
                "You are not allowed to access this resource.");
            
            var challenge = _mapper.Map<CreateChallengeRequest, Challenge>(request);
           
            _dbContext.Challenges.Add(challenge);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<Challenge, ChallengeDto>(challenge);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ChallengeDto> DeleteChallenge(CreateChallengeRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ChallengeDto>> GetChallenges()
    {
        throw new NotImplementedException();
    }

    public async Task<ChallengeDto> JoinChallenge(ChallengeJoinRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<ChallengeDto> ExitChallenge(ChallengeJoinRequest request)
    {
        throw new NotImplementedException();
    }
}