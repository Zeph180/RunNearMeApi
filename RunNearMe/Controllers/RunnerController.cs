using System.Net;
using Application.Interfaces;
using Application.Interfaces.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace RunNearMe.Controllers;

public class RunnerController : ControllerBase
{
    private readonly IRunner _runner;

    public RunnerController(IRunner runner)
    {
        _runner = runner;
    }
    
    public async Task<IActionResult> CreateRunner(RunnerDto runnerDto)
    {
       var response = await _runner.CreateRunner<RunnerDto, Runner>(runnerDto);
       return Ok(response);
    }
}