using ObstaRace.API.Dto;
using ObstaRace.API.Models;

namespace ObstaRace.API.Interfaces.Services;

public interface IUserService
{
    Task<ICollection<UserDto>> GetAllUsers();
    Task<UserDto?> GetUser(int id);
    Task<UserStatsDto> GetUserStats();
    Task<bool> BanUser(int userId);
    Task<bool> UnbanUser(int userId);
    //LOGIN/REGISTER
    Task<UserDto?> RegisterUser(RegisterDto registerDto);
    Task<LoginResponseDto?> LoginUser(LoginDto loginDto);
    Task<UserDto?> UpdateUser(UpdateParticipantDto updateUserDto, int userId);
    
}