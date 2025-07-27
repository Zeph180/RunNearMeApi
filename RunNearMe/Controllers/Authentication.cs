using Application.Interfaces;
using Application.Models.Request.Authentication;
using Application.Wrappers;
using Domain.Models.Request.Account;
using Domain.Models.Request.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace RunNearMe.Controllers;

[Route("api/authentication/[controller]")]
[ApiController]
public class Authentication : ControllerBase
{
    private readonly IAuthentication _authentication;
    
    public Authentication(IAuthentication authentication)
    {
        _authentication = authentication;
    }

    /// <summary>
    /// Responsible for creating an account or signup
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AccountCreateRequest request)
    {
        var response = await _authentication.CreateAccount(request);
        return Ok(response);
    }

    /// <summary>
    /// Api for logging into the application
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authentication.Login(request);
        return Ok(ApiResponse<object>.SuccessResponse(response));
    }
    
    /// <summary>
    /// This is responsible for completing the user profile
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("complete-profile")]
    public async Task<IActionResult> CompleteProfile([FromBody] CompleteProfileReq request)
    {
        var response = await _authentication.CompleteProfile(request);
        return Ok(ApiResponse<object>.SuccessResponse(response));
    }
}