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

    [HttpPost("/upload-file")]
    public async Task<IActionResult> UploadFile([FromForm] FileUploadRequest request)
    {
       var response = await _cloudinaryService.UploadFileAsync(Request.Form.Files[0], request);
       return Ok(ApiResponse<object>.SuccessResponse(response));
    }
    
    [HttpPost("/upload-image")]
    public async Task<IActionResult> UploadImage([FromForm] ImageUploadRequest request)
    {
        var response = await _cloudinaryService.UploadImageAsync(request);
        return Ok(ApiResponse<object>.SuccessResponse(response));
    }

    [HttpPost("/create-challenge")]
    public async Task<IActionResult> CreateChallenge([FromForm] CreateChallengeRequest request)
    {
        _logger.LogInformation("Creating challenge");
        var result = await _challengeRepository.CreateChallenge(request);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }
}