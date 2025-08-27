using Application.Errors;
using Application.Interfaces;
using Application.Middlewares.ErrorHandling;
using Application.Models.Request.Cloudinary;
using Application.Models.Request.Posts;
using Application.Services;
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
    private readonly ICloudinaryService  _cloudinaryService ;

    public PostRepository(
        ILogger<IPostRepository> logger, 
        IConfiguration configuration, 
        IPeopleHelper peopleHelper, 
        IMapper mapper,
        AppDbContext context,
        ICloudinaryService cloudinary)
    {
        _logger = logger;
        _configuration = configuration;
        _peopleHelper = peopleHelper;
        _mapper = mapper;
        _context = context;
        _cloudinaryService  = cloudinary;
    }

    public async Task<CreatePostResponse> CreatePost(CreatePostRequest request)
    {
        try
        {
            _logger.LogInformation("Start create post method");
            var runner = await _peopleHelper.GetValidProfileAsync(request.RunnerId, ErrorCodes.PersonNotFound,
                ErrorMessages.PersonNotFound);
            
            var postRequest = _mapper.Map<CreatePostRequest, Post>(request);

            if (request.PostFile != null)
            {
                _logger.LogInformation("Uploading post file {PostId} {RunnerId}", postRequest.PostId, runner.RunnerId);
                var imageUploadReq = new ImageUploadRequest
                {
                    Image = request.PostFile,
                    Folder = "Posts"
                };
                var fileUploadResponse = await _cloudinaryService .UploadImageAsync(imageUploadReq);

                if (fileUploadResponse == null )
                {
                    _logger.LogInformation("Failed to upload post file {PostId} {RunnerId}", postRequest.PostId, runner.RunnerId);
                    throw new BusinessException(ErrorMessages.FileUploadFailed, ErrorCodes.FileUploadFailed);
                }
                
                if (!fileUploadResponse.Success)
                {
                    _logger.LogError("Failed to upload challenge art for {runnerId} : {ErrorMessage}",  request.RunnerId,  fileUploadResponse.ErrorMessage);
                    throw new BusinessException(fileUploadResponse.ErrorMessage, ErrorCodes.FileUploadFailed);
                }
                _logger.LogInformation("Post file uploaded {PostId} {RunnerId}", postRequest.PostId, runner.RunnerId);
                postRequest.ImageUrl = fileUploadResponse.Url;
            }
            
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

    public async Task<CreatePostResponse> UpdatePost(UpdatePostRequest request)
    {
        try
        {
            _logger.LogInformation("Start update post method");
            var post = await GetPostAsync(request.PostId, request.RunnerId, true, true);
           
            _logger.LogInformation("Proceeding to update post {PostId} {RunnerId}", post.PostId, post.RunnerId);
            _mapper.Map(request, post);
            await _context.SaveChangesAsync();
            
            var updatedPost = await GetPostAsync(request.PostId, request.RunnerId, true, true);
            _logger.LogInformation("Post updated {PostId} {RunnerId}", updatedPost.PostId, updatedPost.RunnerId);
            return _mapper.Map<Post, CreatePostResponse>(updatedPost);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating post for {RunnerId}", request.RunnerId);
            throw;
        }
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
    
    private async Task<Post?> GetPostAsync(
        Guid postId,
        Guid runnerId,
        bool isAdmin = false,
        bool isActive = true,
        bool includeLikes = false,
        bool includeComments = false)
    {
        try
        {
            _logger.LogInformation("Fetching post. PostId: {PostId}, RunnerId: {RunnerId}, IsAdmin: {IsAdmin}, IsActive: {IsActive}, IncludeLikes: {IncludeLikes}, IncludeComments: {IncludeComments}", 
                postId, runnerId, isAdmin, isActive, includeLikes, includeComments);

            IQueryable<Post> query = _context.Posts.AsQueryable();

            if (includeLikes)
            {
                query = query.Include(p => p.Likes);
            }

            if (includeComments)
            {
                query = query.Include(p => p.Comments);
            }

            if (isAdmin)
            {
                query = query.Where(p => p.RunnerId == runnerId);
            }

            if (isActive)
            {
                query = query.Where(p => !p.Deleted);
            }

            var post = await query.FirstOrDefaultAsync(p => p.PostId == postId);
            
            if (post == null)
            {
                _logger.LogInformation(
                    "Post not found. PostId: {PostId}, RunnerId: {RunnerId}, IsAdmin: {IsAdmin}, IsActive: {IsActive}, IncludeLikes: {IncludeLikes}, IncludeComments: {IncludeComments}", postId, runnerId, isAdmin, isActive, includeLikes, includeComments);
                throw new BusinessException(ErrorMessages.PostNotFound, ErrorCodes.ResourceNotFound);
            }
            
            return post; 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching post. PostId: {PostId}, RunnerId: {RunnerId}", postId, runnerId);
            throw;
        }
    }
}