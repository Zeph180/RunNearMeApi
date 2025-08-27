using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request.Posts;

public class UpdatePostRequest
{
    public Guid PostId { get; set; }
    public Guid RunnerId { get; set; }
    public required string Caption { get; set; }
    [MaxLength(100)]
    [Url]
    public string? ImageUrl { get; set; }
    public required string Location { get; set; }
}