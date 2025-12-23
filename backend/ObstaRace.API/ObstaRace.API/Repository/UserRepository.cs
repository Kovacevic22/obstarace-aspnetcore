using ObstaRace.API.Data;
using ObstaRace.API.Interfaces;
using ObstaRace.API.Models;

namespace ObstaRace.API.Repository;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    public UserRepository(DataContext context)
    {
        _context = context;
    }
    //--------------USER-----------------//
    //GET
    public ICollection<User> GetAllUsers()
    {
        return _context.Users.OrderBy(u => u.Id).ToList();
    }
    public User GetUser(int id)
    {
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }
    public bool UserExists(int id)
    {
        return _context.Users.Any(u => u.Id == id);
    }
    
}