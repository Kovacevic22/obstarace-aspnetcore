using ObstaRace.API.Dto;

namespace ObstaRace.API.Interfaces.Services;

public interface IUserService
{
    Task<ICollection<UserDto>> GetAllUsers();
    Task<UserDto?> GetUser(int id);
    
    //CRUD
}