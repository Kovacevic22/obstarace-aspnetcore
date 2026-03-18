using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ObstaRace.Application.Interfaces.Services;
using ObstaRace.Infrastructure.Configuration;

namespace ObstaRace.Infrastructure.Service;

public class FileService:IFileService
{
    private readonly IAmazonS3 _s3Client;
    private readonly AwsSettings _settings;
    private readonly ILogger<FileService> _logger;
    public FileService(IAmazonS3 s3Client, IOptions<AwsSettings> settings, ILogger<FileService> logger)
    {
        _s3Client = s3Client;
        _logger = logger;
        _settings = settings.Value;
    }
    public async Task<string> UploadFileAsync(IFormFile file, string folder)
    {
        var fileName = Path.GetFileName(file.FileName).Replace(" ", "-").ToLower();
        var key = $"{folder}/{Guid.NewGuid()}-{fileName}";
        await using var stream = file.OpenReadStream();
        var putObjectRequest = new PutObjectRequest()
        {
            BucketName = _settings.BucketName,
            Key =  key,
            ContentType = file.ContentType,
            InputStream = stream,
        }; 
        await _s3Client.PutObjectAsync(putObjectRequest);
        _logger.LogInformation("Uploaded image {FileName} to bucket {BucketName}", fileName, _settings.BucketName);
        return key;
        
    }

    public Task<string> GetFileAsync(string key)
    {
        return Task.FromResult($"{_settings.BaseUrl}/{key}");
    }

    public async Task<bool> DeleteFileAsync(string key)
    {
        var deleteObject = new DeleteObjectRequest 
        {
            BucketName = _settings.BucketName, 
            Key = key
        }; 
        await _s3Client.DeleteObjectAsync(deleteObject); 
        _logger.LogInformation("Deleted image {FileName}", key); 
        return true;
    }
}