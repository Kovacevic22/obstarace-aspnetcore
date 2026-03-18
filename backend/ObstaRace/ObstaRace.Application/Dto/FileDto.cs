using Microsoft.AspNetCore.Http;

namespace ObstaRace.Application.Dto;

public class FileDto
{
    public class ProfileImageUploadDto
    {
        public IFormFile Image { get; set; } = null!;
    }
}