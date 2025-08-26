using Application.Interfaces;
using Application.Models.Request.Posts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Repository.Repositories;

public class PostRepository : IPostRepository
{
    private readonly ILogger<IPostRepository> _logger;
    private readonly IConfiguration _configuration;
    private readonly IPeopleHelper _peopleHelper;

    public PostRepository(ILogger<IPostRepository> logger, IConfiguration configuration, IPeopleHelper peopleHelper)
    {
        _logger = logger;
        _configuration = configuration;
        _peopleHelper = peopleHelper;
    }

    public async Task<CreatePostResponse> CreatePost(CreatePostRequest request)
    {
        try
        {
            _logger.LogInformation("Start create post method");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<CreatePostResponse> UpdatePost(CreatePostRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<CreatePostResponse> DeletePost(CreatePostRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<CreatePostResponse> React(CreatePostRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<CreatePostResponse> Comment(CreatePostRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<CreatePostResponse> SharePost(CreatePostRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<GetPostResponse> GetPostById(Guid postId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<GetPostResponse>> GetPostsByUser(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<GetPostResponse>> GetFeed(Guid userId, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public async Task<List<CommentResponse>> GetComments(Guid postId)
    {
        throw new NotImplementedException();
    }

    public async Task<CreatePostResponse> AddMediaToPost(Guid postId, IFormFile media)
    {
        throw new NotImplementedException();
    }

    public async Task<CreatePostResponse> RemoveMediaFromPost(Guid postId, string mediaId)
    {
        throw new NotImplementedException();
    }
}