using Application.Interfaces;
using Application.Interfaces.Dtos.Run;
using Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RunNearMe.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RunController : ControllerBase
{
    private readonly IRun _runService;

    public RunController(IRun runService)
    {
        _runService = runService;
    }
    

    [HttpGet("createRun/{runnerId}")]
    public async Task<IActionResult> CreateRun([FromRoute] Guid runnerId)
    {
       var response = await _runService.CreateRun(runnerId);
       return Ok(ApiResponse<object>.SuccessResponse(response));
    }

    [HttpPost("updateRun")]
    public async Task<IActionResult> UpdateRun([FromBody] RunDto request)
    {
        await _runService.UpdateRun(request);
        return Ok(ApiResponse<object>.SuccessResponse(request));
    }
}