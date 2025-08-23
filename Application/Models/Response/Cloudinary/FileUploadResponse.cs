namespace Application.Models.Response.Cloudinary;

public class FileUploadResponse
{
    public bool Success { get; set; }
    public string? PublicId { get; set; }
    public string? Url { get; set; }
    public string? SecureUrl { get; set; }
    public string? Format { get; set; }
    public string? ResourceType { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public long Bytes { get; set; }
    public List<string>? Tags { get; set; }
    public Dictionary<string, string>? Context { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ErrorMessage { get; set; }
}