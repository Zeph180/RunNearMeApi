using Application.Interfaces;
using Application.Interfaces.Dtos.Run;
using Application.Models.Request.Run;
using Application.Models.Response.Run;
using Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RunNearMe.Controllers;
[Route("api/[controller]")]
[ApiController]
// [Authorize]
public class RunController : ControllerBase
{
    private readonly IRunRepository _runRepositoryService;
    private readonly ILogger<RunController> _logger;

    public RunController(IRunRepository runRepositoryService,  ILogger<RunController> logger)
    {
        _runRepositoryService = runRepositoryService;
        _logger = logger;
    }
    

    [HttpGet("create-run/{runnerId}")]
    public async Task<IActionResult> CreateRun([FromRoute] Guid runnerId)
    {
        _logger.LogInformation("CreateRun");
       var response = await _runRepositoryService.StartRun(runnerId);
       return Ok(ApiResponse<object>.SuccessResponse(response));
    }

    [HttpPost("update-run")]
    public async Task<IActionResult> UpdateRun([FromBody] RunDto request)
    {
        _logger.LogInformation("Update-run");
        await _runRepositoryService.CompleteRun(request);
        return Ok(ApiResponse<object>.SuccessResponse(request));
    }

    [HttpPut("add-route-point")]
    public async Task<IActionResult> AddRoutPoint([FromBody] AddRoutePointRequest request)
    {
        _logger.LogInformation("UpdateRun");
        await _runRepositoryService.AddRoutePoint(request);
        return Ok(ApiResponse<object>.SuccessResponse(request));
    }

    [HttpGet("get-nearby-runs")]
    public async Task<IActionResult> GetRunsNearLocationAsync([FromQuery] NearByRunRequest request)
    {
        _logger.LogInformation("GetNearByRuns");
        await _runRepositoryService.GetRunsNearLocationAsync(request);
        return Ok(ApiResponse<object>.SuccessResponse(request));
    }
}