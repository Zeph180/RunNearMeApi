using Application.Interfaces.Dtos.Post;
using Application.Models.Request.Posts;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IPostRepository
{
    Task<CreatePostResponse> CreatePost(CreatePostRequest request);
    Task<CreatePostResponse> UpdatePost(UpdatePostRequest request);
    Task<bool> DeletePost(Guid postId, Guid userId);
    Task<ReactResponse> React(ReactRequest request);
    Task<CommentDto> Comment(CommentRequest request);
    Task<CreatePostResponse> SharePost(CreatePostRequest request);
    Task<PostDto> GetPostById(GetPostRequest request);
    Task<List<GetPostResponse>> GetPostsByUser(Guid userId);
    Task<List<GetPostResponse>> GetFeed(Guid userId, int pageNumber, int pageSize);
    Task<List<CommentResponse>> GetComments(Guid postId);
    Task<CreatePostResponse> AddMediaToPost(Guid postId, IFormFile media);
    Task<CreatePostResponse> RemoveMediaFromPost(Guid postId, string mediaId);
}