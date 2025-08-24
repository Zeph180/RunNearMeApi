using Application.Interfaces;
using Application.Models.Request.Challenge;
using Application.Models.Request.Cloudinary;
using Application.Services;
using Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RunNearMe.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ChallengeController : ControllerBase
{
    private readonly ILogger<ChallengeController> _logger;
    private readonly IChallengeRepository _challengeRepository;
    private readonly ICloudinaryService _cloudinaryService;
    
    public ChallengeController(ILogger<ChallengeController> logger, IChallengeRepository challengeRepository, ICloudinaryService _cloudinaryService)
    {
        _logger = logger;
        _challengeRepository = challengeRepository;
        this._cloudinaryService = _cloudinaryService;
    }

    /// <summary>
    /// Method for creating a challenge
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("/create-challenge")]
    public async Task<IActionResult> CreateChallenge([FromForm] CreateChallengeRequest request)
    {
        _logger.LogInformation("Creating challenge");
        var result = await _challengeRepository.CreateChallenge(request);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    /// <summary>
    /// Responsible for updating the textual details of a challenge
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("/update-challenge-details")]
    public async Task<IActionResult> UpdateChallengeDetails([FromForm] UpdateChallengeRequest request)
    {
        _logger.LogInformation("Updating challenge");
        var result = await _challengeRepository.UpdateChallengeDetails(request);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    /// <summary>
    /// This endpoint is responsible for updating challenge art image
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("/update-challenge-art")]
    public async Task<IActionResult> UpdateChallengeArt([FromForm] UpdateChallangeArtRequest request)
    {
        _logger.LogInformation("Updating challenge art");
        var result = await _challengeRepository.UpdateChallengeArt(request);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }
    
    /// <summary>
    /// Method for deleting a challenge
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpDelete("/delete-challenge")]
    public async Task<IActionResult> DeleteChallenge([FromBody] ChallengeJoinRequest request)
    {
        _logger.LogInformation("Deleting challenge");
        var result = await _challengeRepository.DeleteChallenge(request);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }
    
    /// <summary>
    /// Gets all active challenges for a user
    /// Boolean determines joined or not joined
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("/get-active-challenges")]
    public async Task<IActionResult> GetActiveChallenges([FromQuery] GetChallengesRequest request)
    {
        _logger.LogInformation("Getting active challenges");
        var result = await _challengeRepository.GetActiveChallenges(request);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }
    
    /// <summary>
    /// Endpoint for joining a challenge
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("/join-challenge")]
    public async Task<IActionResult> JoinChallenge([FromBody] ChallengeJoinRequest request)
    {
        _logger.LogInformation("Joining challenge");
        var result = await _challengeRepository.JoinChallenge(request);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }
    
    /// <summary>
    /// Endpoint for exiting ar leaving a challenge
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("/exit-challenge")]
    public async Task<IActionResult> ExitChallenge([FromBody] ChallengeJoinRequest request)
    {
        _logger.LogInformation("Exiting challenge");
        var result = await _challengeRepository.ExitChallenge(request);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }
}