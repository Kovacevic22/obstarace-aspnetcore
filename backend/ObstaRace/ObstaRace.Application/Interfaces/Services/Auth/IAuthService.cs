using ObstaRace.Application.Dto;

namespace ObstaRace.Application.Interfaces.Services.Auth;

public interface IAuthService
{
    //LOGIN/REGISTER
    Task<bool> VerifyEmail(string token);
    Task<bool> ResendVerificationEmail(string email);
    Task<UserDto?> RegisterUser(RegisterDto registerDto);
    Task<LoginResponseDto?> LoginUser(LoginDto loginDto);
}