using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.Application.Dto;
using ObstaRace.Application.Interfaces.Services;

namespace ObstaRace.API.Controllers;
[ApiController]
[Route("api/files")]
public class FileController:ControllerBase
{
    private readonly IFileService _fileService;
    private readonly IUserService _userService;
    private readonly ILogger<FileController> _logger;

    public FileController(IFileService fileService, ILogger<FileController> logger, IUserService userService)
    {
        _fileService = fileService;
        _logger = logger;
        _userService = userService;
    }
    //--------PROFILE IMAGES------------//
    [HttpPost("profile-image")]
    [Authorize] 
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadProfileImage([FromForm] FileDto.ProfileImageUploadDto image)
    { 
        if (image.Image.Length == 0)
            return BadRequest(new { error = "No file provided" });
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(image.Image.ContentType))
            return BadRequest(new { error = "Invalid file type (jpeg,png,webp)" });

        if (image.Image.Length > 5 * 1024 * 1024) // 5MB limit
            return BadRequest(new { error = "File too large" });
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _userService.GetUser(userId);
        if (!string.IsNullOrEmpty(user?.ProfileImgKey))
        {
            await _fileService.DeleteFileAsync(user.ProfileImgKey, HttpContext.RequestAborted);
        }
        var key = await _fileService.UploadFileAsync(image.Image,"profile-images", HttpContext.RequestAborted);
        await _userService.UpdateProfileImage(userId, key);
        return Ok(new { key });
    }

    [HttpDelete("profile-image")]
    [Authorize]
    public async Task<IActionResult> DeleteProfileImage()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _userService.GetUser(userId);
        if (user == null || string.IsNullOrEmpty(user.ProfileImgKey))
            return BadRequest(new { error = "User don't have profile image" });
        await _fileService.DeleteFileAsync(user.ProfileImgKey, HttpContext.RequestAborted);
        await _userService.UpdateProfileImage(userId, null!);
        return NoContent();
    }

    [HttpGet("profile-image")]
    [Authorize]
    public async Task<IActionResult> GetProfileImage()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _userService.GetUser(userId);
        if(user == null || string.IsNullOrEmpty(user.ProfileImgKey))
            return BadRequest(new { error = "User don't have profile image" });
        var key = await _fileService.GetFileAsync(user.ProfileImgKey);
        return Ok(new { key });
    }
    //----------------------------------//
}