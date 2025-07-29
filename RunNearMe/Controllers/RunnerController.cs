using System.Net;
using Application.Interfaces;
using Application.Interfaces.Dtos;
using Application.Wrappers;
using Domain.Entities;
using Domain.Models.Request.Account;
using Microsoft.AspNetCore.Mvc;

namespace RunNearMe.Controllers;

[Route("api/[controller]")]
[ApiController]

public class RunnerController : ControllerBase
{
    private readonly IRunner _runner;

    public RunnerController(IRunner runner)
    {
        _runner = runner;
    }
    /// <summary>
    /// This method is responsible for creating new user accounts
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("signup")]
    public async Task<IActionResult> CreateAccount(AccountCreateRequest request)
    {
        var response = await _runner.CreateAccount(request);
        return Ok(ApiResponse<object>.SuccessResponse(response));
    }
}