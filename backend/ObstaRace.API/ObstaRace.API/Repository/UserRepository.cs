using Microsoft.EntityFrameworkCore;
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
    public async Task<ICollection<User>> GetAllUsers()
    {
        return await _context.Users.OrderBy(u => u.Id).ToListAsync();
    }
    public async Task<User?> GetUser(int id)
    {
        return await _context.Users.FindAsync(id);
    }
    public async Task<bool> UserExists(int id)
    {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }
    
}