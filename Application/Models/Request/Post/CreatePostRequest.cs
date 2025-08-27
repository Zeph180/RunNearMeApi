using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Application.Models.Request.Posts;

public class CreatePostRequest
{
    public Guid RunnerId { get; set; }
    public required string Caption { get; set; }
    [MaxLength(100)]
    [Url]
    public IFormFile? PostFile { get; set; }
    public required string Location { get; set; }
}