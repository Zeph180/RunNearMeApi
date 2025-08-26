using Application.Errors;
using Application.Interfaces;
using Application.Models.Request.Posts;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repository.Persistence;

namespace Repository.Repositories;

public class PostRepository : IPostRepository
{
    private readonly ILogger<IPostRepository> _logger;
    private readonly IConfiguration _configuration;
    private readonly IPeopleHelper _peopleHelper;
    private readonly IMapper  _mapper;
    private readonly AppDbContext _context;

    public PostRepository(
        ILogger<IPostRepository> logger, 
        IConfiguration configuration, 
        IPeopleHelper peopleHelper, 
        IMapper mapper,
        AppDbContext context)
    {
        _logger = logger;
        _configuration = configuration;
        _peopleHelper = peopleHelper;
        _mapper = mapper;
        _context = context;
    }

    public async Task<CreatePostResponse> CreatePost(CreatePostRequest request)
    {
        try
        {
            _logger.LogInformation("Start create post method");
            var runner = await _peopleHelper.GetValidProfileAsync(request.RunnerId, ErrorCodes.PersonNotFound,
                ErrorMessages.PersonNotFound);
            
            var postRequest = _mapper.Map<CreatePostRequest, Post>(request); 
            _logger.LogInformation("Proceeding to create post {PostId} {RunnerId}", postRequest.PostId, runner.RunnerId);
            _context.Posts.Add(postRequest);
            await _context.SaveChangesAsync();
            var createdPost = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == postRequest.PostId);
            return _mapper.Map<Post, CreatePostResponse>(createdPost);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating post for {RunnerId}", request.RunnerId);
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