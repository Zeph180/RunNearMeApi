using Application.Interfaces;
using Application.Models.Request.Posts;
using Application.Wrappers;
using Microsoft.AspNetCore.Mvc;

namespace RunNearMe.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly ILogger<PostController> _logger;
    private readonly IPostRepository  _postRepository;

    public PostController(ILogger<PostController> logger, IPostRepository postRepository)
    {
        _logger = logger;
        _postRepository = postRepository;
    }

    [HttpPost("create-post")]
    public async Task<IActionResult> CreatePost([FromForm] CreatePostRequest request)
    {
        _logger.LogInformation("Starting create post controller method");
        var result = await _postRepository.CreatePost(request);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpPut("update-post")]
    public async Task<IActionResult> UpdatePost([FromBody] UpdatePostRequest request)
    {
        _logger.LogInformation("Starting update post controller method");
        var result = await _postRepository.UpdatePost(request);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }
}