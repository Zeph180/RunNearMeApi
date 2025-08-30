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

    [HttpDelete("delete-post")]
    public async Task<IActionResult> DeletePost([FromQuery] Guid runnerId, [FromQuery] Guid postId)
    {
        _logger.LogInformation("start delete post method");
        var response = await _postRepository.DeletePost(postId, runnerId);
        return Ok(ApiResponse<object>.SuccessResponse(response));
    }

    [HttpPost("comment")]
    public async Task<IActionResult> Comment([FromBody] CommentRequest request)
    {
        _logger.LogInformation("Starting comment controller method");
        var result = await _postRepository.Comment(request);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }
    
    [HttpPost("React")]
    public async Task<IActionResult> React([FromBody] ReactRequest request)
    {
        _logger.LogInformation("Starting comment controller method");
        var result = await _postRepository.React(request);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }

    [HttpGet("get-post-by-id")]
    public async Task<IActionResult> GetPostById([FromQuery] GetPostRequest request)
    {
        _logger.LogInformation("Starting get post by id controller method");
        var result = await _postRepository.GetPostById(request);
        return Ok(ApiResponse<object>.SuccessResponse(result));
    }
}