using ObstaRace.API.Dto;
using ObstaRace.API.Models;

namespace ObstaRace.API.Interfaces;

public interface IUserRepository
{
    Task<ICollection<User>> GetAllUsers();
    Task<User?> GetUser(int id);
    Task<bool> UserExists(int id);
    Task<UserStatsDto> GetUserStats();
    //LOGIN/REGISTER
    Task<bool> CreateUser(User user);
    Task<bool> SaveChanges();
    //CRUD
    Task<bool> UpdateUser(User user);
    Task<bool> DeleteUser(int userId);
    Task<User?> GetUserByEmail(string email);
}