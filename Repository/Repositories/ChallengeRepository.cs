using Application.Errors;
using Application.Interfaces;
using Application.Interfaces.Dtos.Challenge;
using Application.Middlewares.ErrorHandling;
using Application.Models.Request.Challenge;
using Application.Models.Request.Cloudinary;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Persistence;

namespace Repository.Repositories;

public class ChallengeRepository : IChallengeRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IPeopleHelper _peopleHelper;
    private readonly ILogger<ChallengeRepository> _logger;
    private readonly ICloudinaryService _cloudinaryService;
    
    public ChallengeRepository(
        AppDbContext dbContext, IMapper mapper, 
        IPeopleHelper peopleHelper, ILogger<ChallengeRepository> logger, 
        ICloudinaryService cloudinaryService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _peopleHelper = peopleHelper;
        _logger = logger;
        _cloudinaryService = cloudinaryService;
    }
    
    public async Task<ChallengeDto> CreateChallenge(CreateChallengeRequest request)
    {
        try
        {
            _logger.LogInformation("Starting to create challenge");
           var runner = await _peopleHelper.GetValidProfileAsync(request.RunnerId, "USER_NOT_ALLOWED",
                "You are not allowed to access this resource.");

           var imageDetails = new ImageUploadRequest
           {
               Image = request.ChallengeArt,
               Folder = "ChallengeArts",
               PublicId = request.Name
           };
            var fileUploadResponse = await _cloudinaryService.UploadImageAsync(imageDetails);

            if (fileUploadResponse == null)
            {
                _logger.LogError("Failed to upload challenge art for {runnerId}",  request.RunnerId);
                throw new BusinessException(ErrorMessages.FileUploadFailed, ErrorCodes.FileUploadFailed);
            }

            if (!fileUploadResponse.Success)
            {
                _logger.LogError("Failed to upload challenge art for {runnerId} : {ErrorMessage}",  request.RunnerId,  fileUploadResponse.ErrorMessage);
                throw new BusinessException(fileUploadResponse.ErrorMessage, ErrorCodes.FileUploadFailed);
            }
            
            var challenge = _mapper.Map<CreateChallengeRequest, Challenge>(request);
            challenge.ImageUrl = fileUploadResponse.Url;
            challenge.PushTopic = GenerateUniqueTopic(request.Name);
           
            _dbContext.Challenges.Add(challenge);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Challenge created successfully");
            return _mapper.Map<Challenge, ChallengeDto>(challenge);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating challenge {ChallengeRequest}", request);;
            throw;
        }
    }

    private static string GenerateUniqueTopic(string prefix)
    {
        return $"{prefix}_{Guid.NewGuid():N}";
    }

    public async Task<bool> DeleteChallenge(ChallengeJoinRequest request)
    {
        try
        {
            _logger.LogInformation("Starting to delete challenge {ChallengeId} {Runner}", request.ChallengeId, request.RunnerId);
            var challenge = await _dbContext.Challenges
                .FirstOrDefaultAsync(c => c.ChallengeId == request.ChallengeId && c.RunnerId == request.RunnerId && c.IsDeleted == false);

            if (challenge == null)
            {
                _logger.LogWarning("Challenge not found. RunnerId: {RunnerId}, ChallengeId: {ChallengeId}", request.RunnerId, request.ChallengeId);
                throw new BusinessException(ErrorMessages.PersonNotFound, ErrorCodes.PersonNotFound);
            }
            challenge.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Challenge deleted successfully {ChallengeId} {Runner}", request.ChallengeId, request.RunnerId);;
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting challenge {ChallengeId} {Runner}", request.ChallengeId, request.RunnerId);
            throw;
        }
    }

    public async Task<List<ChallengeDto>> GetActiveChallenges(GetChallengesRequest request)
    {
        try
        {
            _logger.LogInformation("Getting active challenges");
            return await GetChallengeSListAsync(request.ChallengeId, request.PageNumber, request.PageSize, request.Joined);;
        }
        catch (Exception e)
        { 
            _logger.LogError(e, "Error getting active challenges");
            throw new BusinessException(e.Message, ErrorCodes.ActionNotAllowed);;
        }
    }

    public async Task<ChallengeDto> JoinChallenge(ChallengeJoinRequest request)
    {
        try
        {
            _logger.LogInformation("Starting to join challenge {ChallengeId} {Runner}", request.ChallengeId, request.RunnerId);
            var challenge = await GetChallengeAsync(request.ChallengeId);
            if (challenge == null)
            {
                _logger.LogWarning("Challenge not found. RunnerId: {RunnerId}, ChallengeId: {ChallengeId}", request.RunnerId, request.ChallengeId);
                throw new BusinessException(ErrorMessages.PersonNotFound, ErrorCodes.PersonNotFound);
            }
            
            var alreadyJoined = challenge?.Challengers?.Any(r => r.RunnerId == request.RunnerId);
            if (alreadyJoined.HasValue)
            {
                _logger.LogInformation("Runner already joined the challenge. {RunnerId}, {ChallengeId}", request.RunnerId, request.ChallengeId);
                throw new BusinessException("Runner already joined this challenge", ErrorCodes.ActionNotAllowed);
            }
            
            _logger.LogInformation("Runner joining the challenge. {RunnerId}, {ChallengeId}", request.RunnerId, request.ChallengeId);
            var challenger = await _peopleHelper.GetValidProfileAsync(request.RunnerId, "PROFILE_NOT_FOUND", "Profile not found");
            challenge?.Challengers?.Add(challenger);
            await _dbContext.SaveChangesAsync();   
            _logger.LogInformation("Runner joined the challenge. {RunnerId}, {ChallengeId}", request.RunnerId, request.ChallengeId);
            return _mapper.Map<Challenge, ChallengeDto>(challenge);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error joining challenge {ChallengeId} {Runner}", request.ChallengeId, request.RunnerId);
            throw new BusinessException(ErrorMessages.UnHandledException, ErrorCodes.UnHandledException);
        }
    }

    public async Task<ChallengeDto> ExitChallenge(ChallengeJoinRequest request)
    {
        throw new NotImplementedException();
    }
    
    private async Task<Challenge?> GetChallengeAsync(Guid challengeId) {
        try
        {
            _logger.LogInformation("Getting challenge {challengeId}", challengeId);
            var challenge = await _dbContext.Challenges
                .FirstOrDefaultAsync(c => c.ChallengeId == challengeId);
            
            return challenge;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting challenge {challengeId}", challengeId);
            throw ;
        }
    }
    
    private async Task<List<ChallengeDto>> GetChallengeSListAsync(Guid runnerId, int pageNumber = 1, int pageSize = 10, bool joined = false) {
        try
        {
            _logger.LogInformation("Getting challenge list Page: {pageNumber}, Size: {pageSize}", pageNumber, pageSize);
            var query = joined 
                ? _dbContext.Challenges.Where(c => !c.IsDeleted && c.Challengers.Any(r => r.RunnerId == runnerId) && c.EndsAt > DateTime.UtcNow)
                : _dbContext.Challenges.Where(c => !c.IsDeleted && c.Challengers.Any(r => r.RunnerId != runnerId) && c.EndsAt > DateTime.UtcNow);
            
            var challenge = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Select(c => new ChallengeDto
                {
                    Description = c.Description,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl,
                    Target = c.Target,
                    CreatedAt = c.CreatedAt,
                    EndsAt = c.EndsAt
                })
                .ToListAsync();
            
            return challenge;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting challenges for {RunnerId}", runnerId);
            throw ;
        }
    }
}