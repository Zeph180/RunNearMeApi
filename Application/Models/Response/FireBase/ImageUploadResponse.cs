namespace Application.Models.Response.FireBase;

public class ImageUploadResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? Url { get; set; }
}