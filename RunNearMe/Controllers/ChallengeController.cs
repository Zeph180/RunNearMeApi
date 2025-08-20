using Application.Interfaces;
using Application.Models.Request.Challenge;
using Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RunNearMe.Controllers;
[Route("api/{controller}")]
[ApiController]
public class ChallengeController : ControllerBase
{
    private readonly ILogger<ChallengeController> _logger;
    private readonly IChallengeService _challengeService;
    
    public ChallengeController(ILogger<ChallengeController> logger, IChallengeService challengeService)
    {
        _logger = logger;
        _challengeService = challengeService;
    }

    [HttpPost("/create-challenge")]
    public async Task<IActionResult> CreateChallenge([FromBody] CreateChallengeRequest request)
    {
        _logger.LogInformation("Creating challenge");
        var result = await _challengeService.CreateChallenge(request);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }
}