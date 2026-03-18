using Microsoft.AspNetCore.Http;

namespace ObstaRace.Application.Interfaces.Services;

public interface IFileService
{
    Task<string> UploadFileAsync(IFormFile file, string folderName);
    Task<string> GetFileAsync(string key);
    Task<bool> DeleteFileAsync(string key);
}