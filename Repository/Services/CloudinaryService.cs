using Application.Models.Request.Cloudinary;
using Application.Models.Response.Cloudinary;
using Application.Services;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Repository.Repositories;

public class CloudinaryService : ICloudinaryService
{
    private readonly IConfiguration _config;
    private readonly ILogger<ICloudinaryService> _logger;
    private readonly IMapper _mapper;
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration config, ILogger<ICloudinaryService> logger, IMapper mapper, Cloudinary cloudinary)
    {
        _config = config;
        _logger = logger;
        _mapper = mapper;
        _cloudinary = cloudinary;
    }

    public async Task<FileUploadResponse> UploadFileAsync(IFormFile? file, FileUploadRequest request)
    {
        try
        {
            _logger.LogInformation("Starting to upload file");
            if (file == null || file.Length == 0)
            {
                _logger.LogInformation("File is null or empty");
                return new FileUploadResponse
                {
                    Success = false,
                    ErrorMessage = "File is null or empty"
                };
            }
            
            var maxSize = 50 * 1024 * 1024;
            
            if (file.Length > maxSize)
            {
                return new FileUploadResponse
                {
                    Success = false,
                    ErrorMessage = $"File size exceeds {maxSize} limit"
                };
            }
            
            var uploadParams = CreateUploadParams(file, request);
            
            //Upload to cloudinary
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                _logger.LogError("Error uploading file to cloudinary: {Error}", uploadResult.Error);
                return new FileUploadResponse
                {
                    Success = false,
                    ErrorMessage = uploadResult.Error.Message
                };
            }

            _logger.LogInformation("File uploaded successfully");
            return new FileUploadResponse
            {
                Success = true,
                PublicId = uploadResult.PublicId,
                SecureUrl = uploadResult.SecureUrl?.ToString(),
                Url = uploadResult.Url?.AbsoluteUri,
                Format = uploadResult.Format,
                ResourceType = uploadResult.ResourceType,
                //Width = uploadResult.Width,
                //Height = uploadResult.Height,
                Bytes = uploadResult.Bytes,
                Tags = uploadResult.Tags?.ToList(),
                //Context = uploadResult.Context as Dictionary<string, string>,
                CreatedAt = uploadResult.CreatedAt,
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error uploading file to cloudinary");
            return new FileUploadResponse
            {
                Success = false,
                ErrorMessage = $"Upload failed {e.Message}"
            };
        }
    }

    public async Task<FileUploadResponse> UploadImageAsync(IFormFile image, string? folder = null, string? publicId = null)
    {
        var request = new FileUploadRequest
        {
            File = image,
            Folder = folder,
            PublicId = publicId,
            UseFilename = true,
            UniqueFilename = true,
            Tags = new List<string> { "image" },
            Context = new Dictionary<string, string> { { "type", "image" } }
        };
        return await UploadFileAsync(image, request);
    }

    private static RawUploadParams CreateUploadParams(IFormFile file, FileUploadRequest request)
    {
        var uploadParams = new RawUploadParams()
        {
            File = new FileDescription(file.FileName, file.OpenReadStream()),
            UseFilename = request.UseFilename ?? false,
            UniqueFilename = request.UniqueFilename ?? false,
            Overwrite = false
        };
        
        if (!string.IsNullOrEmpty(request.Folder)) 
            uploadParams.Folder = request.Folder;
        
        if (!string.IsNullOrEmpty(request.PublicId)) 
            uploadParams.PublicId = request.PublicId;
        
        if (request.Tags != null && request.Tags.Any()) 
            uploadParams.Tags = string.Join(",", request.Tags);
        
        // if (request.Context != null && request.Context.Any())
        //     uploadParams.Context = request.Context;
        
        //Set resource type base on file content type
        if (IsImageFile(file.ContentType))
        {
            var imageParams = new ImageUploadParams()
            {
                File = uploadParams.File,
                PublicId = uploadParams.PublicId,
                Folder = uploadParams.Folder,
                UseFilename = request.UseFilename ?? false,
                UniqueFilename = request.UniqueFilename ?? false,
                Tags = uploadParams.Tags,
                Context = uploadParams.Context,
                Overwrite = uploadParams.Overwrite,
            };
            return imageParams;
        }
        else if (IsVideoFile(file.ContentType))
        {
            var videoParam = new VideoUploadParams()
            {
                File = uploadParams.File,
                PublicId = uploadParams.PublicId,
                Folder = uploadParams.Folder,
                UseFilename = request.UseFilename ?? false,
                UniqueFilename = request.UniqueFilename ?? false,
                Tags = uploadParams.Tags,
                // Context = uploadParams.Context,
                Overwrite = uploadParams.Overwrite,
            };
            return videoParam;
        }
        return uploadParams;
    }
    
    private static bool IsImageFile(string contentType)
    {
        var imageTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp", "image/bmp", "image/tiff" };
        return imageTypes.Contains(contentType.ToLower());
    }
    
    private static bool IsVideoFile(string contentType)
    {
        var videoTypes = new[] { "video/mp4", "video/avi", "video/wmv" };
        return videoTypes.Contains(contentType.ToLower());
    }
}