using System.Net;
using Application.Interfaces;
using Application.Interfaces.Dtos;
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
    
    [HttpPost("signup")]
    public async Task<IActionResult> CreateRunner(RunnerDto runnerDto)
    {
       var response = await _runner.CreateRunner<RunnerDto, Runner>(runnerDto);
       return Ok(response);
    }

    [HttpPost("createAccount")]
    public async Task<IActionResult> CreateAccount(AccountCreateRequest request)
    {
        var response = await _runner.CreateAccount(request);
        return Ok(response);
    }
}