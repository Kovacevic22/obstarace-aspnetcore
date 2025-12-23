using ObstaRace.API.Models;

namespace ObstaRace.API.Interfaces;

public interface IUserRepository
{
    ICollection<User> GetAllUsers();
    User GetUser(int id);
    bool UserExists(int id);
}