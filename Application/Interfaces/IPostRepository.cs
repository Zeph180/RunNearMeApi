using Application.Models.Request.Posts;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IPostRepository
{
    Task<CreatePostResponse> CreatePost(CreatePostRequest request);
    Task<CreatePostResponse> UpdatePost(UpdatePostRequest request);
    Task<CreatePostResponse> DeletePost(CreatePostRequest request);
    Task<CreatePostResponse> React(CreatePostRequest request);
    Task<CreatePostResponse> Comment(CreatePostRequest request);
    Task<CreatePostResponse> SharePost(CreatePostRequest request);
    Task<GetPostResponse> GetPostById(Guid postId);
    Task<List<GetPostResponse>> GetPostsByUser(Guid userId);
    Task<List<GetPostResponse>> GetFeed(Guid userId, int pageNumber, int pageSize);
    Task<List<CommentResponse>> GetComments(Guid postId);
    Task<CreatePostResponse> AddMediaToPost(Guid postId, IFormFile media);
    Task<CreatePostResponse> RemoveMediaFromPost(Guid postId, string mediaId);
}