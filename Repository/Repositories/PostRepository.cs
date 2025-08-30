using Application.Errors;
using Application.Interfaces;
using Application.Interfaces.Dtos.Post;
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
                    Folder = "Posts",
                    Additional = new Dictionary<string, string>
                    {
                        {"Method", "CreatePost"},
                        {"PostId", postRequest.PostId.ToString()},
                        {"RunnerId", postRequest.RunnerId.ToString()}
                    }
                };
                var fileUploadResponse = await _cloudinaryService .UploadImageAsync(imageUploadReq);
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
            var post = await GetPostAsync(request.PostId, request.RunnerId, true, true, track: true);
           
            if (request.PostFile != null)
            {
                _logger.LogInformation("Uploading post file {PostId} {RunnerId}", request.PostId, request.RunnerId);
                var imageUploadReq = new ImageUploadRequest
                {
                    Image = request.PostFile,
                    Folder = "Posts",
                    Additional = new Dictionary<string, string>
                    {
                        {"Method", "CreatePost"},
                        {"PostId", request.PostId.ToString()},
                        {"RunnerId", request.RunnerId.ToString()}
                    }
                };
                var fileUploadResponse = await _cloudinaryService .UploadImageAsync(imageUploadReq);
                post.ImageUrl = fileUploadResponse.Url;
            }
            
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
    
    public async Task<bool> DeletePost(Guid postId, Guid runnerId)
    {
        try
        {
            _logger.LogInformation("Start delete post method {PostId} {RunnerId}", postId, runnerId);
            var post = await GetPostAsync(postId, runnerId, true, true, track: true);;
            post.IsDeleted = true;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Post deleted {PostId} {RunnerId}", post.PostId, post.RunnerId);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting post for {PostId} {RunnerId}", postId, runnerId);
            throw;
        }
    }

    public async Task<CommentDto> Comment(CommentRequest request)
    {
        try
        {
            _logger.LogInformation("Start comment repository method {RunnerId} {PostId}", request.RunnerId, request.PostId);
            
            var runner = await _peopleHelper.GetValidProfileAsync(request.RunnerId, ErrorMessages.PersonNotFound, ErrorCodes.PersonNotFound);
            _context.Attach(runner);
            _logger.LogInformation("Runner found RunnerId: {RunnerId}", request.RunnerId);
            _logger.LogInformation("Proceeding to get post {PostId} {RunnerId}", request.PostId, request.RunnerId);
            
            var post = await GetPostAsync(request.PostId, request.RunnerId);
            _context.Attach(post);
            _logger.LogInformation("Post found PostId: {PostId}", request.PostId);
            
            var comment = _mapper.Map<CommentRequest, Comment>(request);
            comment.Post = post;
            comment.Runner = runner;
            _context.Comments.Add(comment);
            
            await _context.SaveChangesAsync();
            _logger.LogInformation("Comment created {CommentId} {RunnerId}", comment.CommentId, request.RunnerId);
            return _mapper.Map<Comment, CommentDto>(comment);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error commenting for {RunnerId} {PostId}", request.RunnerId, request.PostId);
            throw;
        }
    }
    
    public async Task<ReactResponse> React(ReactRequest request)
    {
        try
        {
            _logger.LogInformation("Start react repository method {RunnerId} {PostId}", request.RunnerId, request.PostId);
            _logger.LogInformation("Proceeding to get post {PostId} {RunnerId}", request.PostId, request.RunnerId);
            var post = (await GetPostAsync(request.PostId, request.RunnerId, includeLikes: true))!;
            _logger.LogInformation("Post found PostId: {PostId}", request.PostId);
            _context.Attach(post);
            var runner = await _peopleHelper.GetValidProfileAsync(request.RunnerId, ErrorMessages.PersonNotFound, ErrorCodes.PersonNotFound);
            _context.Attach(runner);
            var like = new Like
            {
                PostId = request.PostId,
                RunnerId = request.RunnerId,
                Post = post,
                Liker = runner
            };
            post.Likes ??= new List<Like>();
            bool alreadyLiked = post.Likes.Any(l => l.RunnerId == request.RunnerId);
            if (alreadyLiked)
            {
                _logger.LogInformation("Runner {RunnerId} already liked post {PostId}", request.RunnerId, request.PostId);
                return new ReactResponse { Reacted = false };
            }
            post.Likes.Add(like);
            await _context.SaveChangesAsync();
            return new ReactResponse { Reacted = true };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error reacting for {RunnerId} {PostId}", request.RunnerId, request.PostId);
            throw;
        }
    }
    
    public async Task<CreatePostResponse> SharePost(CreatePostRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<PostDto> GetPostById(GetPostRequest request)
    {
        try
        {
            _logger.LogInformation("Start get post by id method {PostId} {RunnerId}", request.PostId, request.RunnerId);
            var post = await GetPostAsync(request.PostId, request.RunnerId, includeLikes: true , includeComments: true, includePoster: true);
            _logger.LogInformation("Post found PostId: {PostId}", request.PostId);
            return _mapper.Map<Post, PostDto>(post);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting post by id for {RunnerId} {PostId}", request.RunnerId, request.PostId);
            throw;
        }
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
        bool includeComments = false,
        bool includePoster =  false,
        bool track = false,
        int commentPage = 1,
        int commentPageSize = 10)
    {
        try
        {
            _logger.LogInformation("Fetching post. PostId: {PostId}, RunnerId: {RunnerId}, CommentPage: {CommentPage}, CommentPageSize: {CommentPageSize}", 
                postId, runnerId, commentPage, commentPageSize);

            IQueryable<Post> query = _context.Posts.AsQueryable();
            query = query.Where(p => p.PostId == postId);
            if (!track) query = query.AsNoTracking();

            query = query.AsSplitQuery();
            
            if (includeLikes) query = query.Include(p => p.Likes);

            if (includeComments)
            {
                query = query.Include(p => p.Comments
                    .Where(c => c.ParentCommentId == null)
                    .OrderByDescending(c => c.CreatedAt)
                    .Skip((commentPage - 1) * commentPageSize)
                    .Take(commentPageSize));
            }
            
            if (includePoster) query = query.Include(p => p.Poster);

            if (isAdmin) query = query.Where(p => p.RunnerId == runnerId);
            
            if (isActive)
            {
                query = query.Where(p => !p.IsDeleted);
            }

            var post = await query.FirstOrDefaultAsync();
            
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