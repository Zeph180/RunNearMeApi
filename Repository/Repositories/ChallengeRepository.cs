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
               PublicId = runner.RunnerId.ToString()
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