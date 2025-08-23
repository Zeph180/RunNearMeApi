using Microsoft.AspNetCore.Http;

namespace Application.Models.Request.Cloudinary;

public class FileUploadRequest
{
    public required IFormFile File { get; set; }
    public string? Folder { get; set; }
    public string? PublicId { get; set; }
    public List<string>? Tags { get; set; }
    public Dictionary<string, string>? Context { get; set; }
    public bool? UseFilename { get; set; }
    public bool? UniqueFilename { get; set; }
    public bool? Transformations { get; set; }
}