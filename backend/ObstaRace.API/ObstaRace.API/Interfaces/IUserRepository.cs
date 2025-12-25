using ObstaRace.API.Models;

namespace ObstaRace.API.Interfaces;

public interface IUserRepository
{
    Task<ICollection<User>> GetAllUsers();
    Task<User?> GetUser(int id);
    Task<bool> UserExists(int id);
}