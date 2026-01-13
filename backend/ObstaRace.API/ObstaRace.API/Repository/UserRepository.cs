using Microsoft.EntityFrameworkCore;
using ObstaRace.API.Data;
using ObstaRace.API.Dto;
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
        return await _context.Users
            .Include(u => u.Participant)
            .Include(u => u.Organiser)
            .OrderBy(u => u.Id)
            .ToListAsync();
    }
    public async Task<User?> GetUser(int id)
    {
        return await _context.Users
            .Include(u=>u.Participant).ThenInclude(p=>p.Registrations)
            .Include(u => u.Organiser)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    public async Task<bool> UserExists(int id)
    {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }

    public async Task<UserStatsDto> GetUserStats()
    {
        return new UserStatsDto()
        {
            BannedUsers = await _context.Users.CountAsync(u => u.Banned),
            TotalUsers = await _context.Users.CountAsync()
        };
    }

    public async Task<bool> BanUser(int userId)
    {
        var user =  await _context.Users.FindAsync(userId);
        if (user == null) return false;
        user.Banned = true;
        return await SaveChanges();
    }
    public async Task<bool> UnbanUser(int userId)
    {
        var user =  await _context.Users.FindAsync(userId);
        if (user == null) return false;
        user.Banned = false;
        return await SaveChanges();
    }
    public async Task<bool> CreateUser(User user)
    {
         await _context.Users.AddAsync(user);
         return await SaveChanges();
    }
    public async Task<bool> UpdateUser(User user)
    {
        _context.Users.Update(user);
        return await SaveChanges();
    }

    public async Task<bool> DeleteUser(int userId)
    {
        var deletedRows = await _context.Users
            .Where(u => u.Id == userId)
            .ExecuteDeleteAsync();
        return deletedRows > 0;
    }

    public async Task<bool> SaveChanges()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0;
    }
    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users
            .Include(u => u.Organiser)
            .Include(u => u.Participant)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }
}