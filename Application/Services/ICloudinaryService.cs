using Application.Models.Request.Cloudinary;
using Application.Models.Response.Cloudinary;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public interface ICloudinaryService
{
    Task<FileUploadResponse> UploadFileAsync(IFormFile file, FileUploadRequest request);
    Task<FileUploadResponse> UploadImageAsync(ImageUploadRequest request);
    Task<bool> DeleteFileAsync(string publicId, string? resourceType = null);
    
}