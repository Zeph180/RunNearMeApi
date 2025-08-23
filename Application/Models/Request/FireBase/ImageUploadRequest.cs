using Microsoft.AspNetCore.Http;

namespace Application.Models.Request.FireBase;

public class ImageUploadRequest
{
    public required IFormFile Image { get; set; }
    public string? FolderPath { get; set; } = "images";
    public string? CustomFileName { get; set; }
}