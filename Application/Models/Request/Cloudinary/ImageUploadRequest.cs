using Microsoft.AspNetCore.Http;

namespace Application.Models.Request.Cloudinary;

public class ImageUploadRequest
{
    public required IFormFile Image { get; set; }
    public string? Folder { get; set; }
    public string? PublicId { get; set; }
    public required  Dictionary<string, string> Additional {get; set;}
}