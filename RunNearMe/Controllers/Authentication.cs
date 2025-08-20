using System.Security.Claims;
using Application.Interfaces;
using Application.Models.Request.Authentication;
using Application.Wrappers;
using Domain.Entities;
using Domain.Models.Request.Account;
using Domain.Models.Request.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace RunNearMe.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class Authentication : ControllerBase
{
    private readonly IAuthentication _authentication;
    private readonly ILogger<Authentication> _logger;

    public Authentication(
        IAuthentication authentication, ILogger<Authentication> logger,
        IConfiguration configuration, LinkGenerator linkGenerator)
    {
        _authentication = authentication;
        _logger = logger;
    }

    /// <summary>
    /// Responsible for creating an account or signup
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] AccountCreateRequest request)
    {
        var response = await _authentication.CreateAccount(request);
        return Ok(ApiResponse<object>.SuccessResponse(response));
    }

    /// <summary>
    /// Api for logging into the application
    /// </summary>
    /// <param name="request"></param>
    /// <param name="authorization"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authentication.Login(request);
        return Ok(ApiResponse<object>.SuccessResponse(response));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="authorization"></param>
    /// <returns></returns>
    [HttpGet("get-social-login-details")]
    public async Task<IActionResult> LoginThirdParty([FromHeader(Name = "Authorization")] string authorization)
    {
        var response = await _authentication.Login(authorization);
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="returnUrl"></param>
    /// <returns></returns>
    [HttpGet("login-google")]
    [AllowAnonymous]
    public IActionResult Login([FromQuery] string returnUrl = "/")
    {
        // Store return URL in session for later use
        HttpContext.Session.SetString("ReturnUrl", returnUrl);
        HttpContext.Session.SetString("AuthState", Guid.NewGuid().ToString());

        var properties = new AuthenticationProperties
        {
            RedirectUri = $"{Request.Scheme}://{Request.Host}/api/Authentication/google-callback",
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(15),
            Items =
            {
                { "LoginProvider", GoogleDefaults.AuthenticationScheme },
                { "scheme", GoogleDefaults.AuthenticationScheme }
            }
        };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet("google-callback")]
    [AllowAnonymous]
    public async Task<IActionResult> GoogleCallBack()
    {
        var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        var returnUrl =  HttpContext.Session.GetString("ReturnUrl");

        if (!result.Succeeded)
        {
            foreach (var error in result.Properties?.Items ?? new Dictionary<string, string?>())
            {
                _logger.LogWarning("Google auth error: {Key} = {Value}", error.Key, error.Value);
            }

            return BadRequest(ApiResponse<object>.FailResponse("Google authentication failed"));
        }

        var googleUser = result.Principal;
        var email = googleUser.FindFirst(ClaimTypes.Email)?.Value ?? googleUser.FindFirst("emailaddress")?.Value;
        var name = googleUser.FindFirst(ClaimTypes.Name)?.Value ?? googleUser.FindFirst("name")?.Value;

        var user = new AccountCreateRequest
        {
            Email = email,
            Name = name,
            Password = ""
        };

        var loginResponse = await _authentication.LoginWithGoogle(user);

        Response.Cookies.Append(
            "RunNearMe.AuthData", loginResponse, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(5)
            });

        return Redirect($"{returnUrl}?token={loginResponse}");
    }
}