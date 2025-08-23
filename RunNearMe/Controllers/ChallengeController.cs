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
    private readonly IChallengeService _challengeService;
    private readonly ICloudinaryService _cloudinaryService;
    
    public ChallengeController(ILogger<ChallengeController> logger, IChallengeService challengeService, ICloudinaryService _cloudinaryService)
    {
        _logger = logger;
        _challengeService = challengeService;
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
        var response = await _cloudinaryService.UploadImageAsync(request.Image, request.Folder, request.PublicId);
        return Ok(ApiResponse<object>.SuccessResponse(response));
    }

    [HttpPost("/create-challenge")]
    public async Task<IActionResult> CreateChallenge([FromBody] CreateChallengeRequest request)
    {
        _logger.LogInformation("Creating challenge");
        var result = await _challengeService.CreateChallenge(request);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }
}