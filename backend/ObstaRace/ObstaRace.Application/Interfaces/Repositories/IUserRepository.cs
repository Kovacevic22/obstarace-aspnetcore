

using ObstaRace.Application.Dto;
using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<ICollection<User>> GetAllUsers(int? page, int? pageSize);
    Task<User?> GetUser(int id);
    Task<bool> UserExists(int id);
    Task<UserStatsDto> GetUserStats();
    Task<bool> BanUser(int userId);
    Task<bool> UnbanUser(int userId);
    //LOGIN/REGISTER
    Task<bool> CreateUser(User user);
    Task<bool> SaveChanges();
    //CRUD
    Task<bool> UpdateUser(User user);
    Task<bool> DeleteUser(int userId);
    Task<User?> GetUserByEmail(string email);
}