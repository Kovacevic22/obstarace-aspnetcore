using Microsoft.EntityFrameworkCore;
using ObstaRace.API.Data;
using ObstaRace.API.Interfaces;
using ObstaRace.API.Models;

namespace ObstaRace.API.Repository;

public class RegistrationRepository : IRegistrationRepository
{
    private DataContext _context;
    public RegistrationRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Registration>> GetAllRegistrations()
    {
        return await _context.Registrations.OrderBy(r => r.Id).ToListAsync();
    }

    public async Task<Registration?> GetRegistration(int id)
    {
        return await _context.Registrations.FindAsync(id);
    }
    public async Task<bool> RegistrationExists(int id)
    {
        return await _context.Registrations.AnyAsync(r => r.Id == id);
    }
    //CRUD
    public async Task<bool> CreateRegistration(Registration registration)
    {
        _context.Registrations.Add(registration);
        return await SaveChanges();
    }
    public async Task<bool> UpdateRegistration(Registration registration)
    {
        _context.Registrations.Update(registration);
        return await SaveChanges();
    }
    public async Task<bool> DeleteRegistration(int registrationId)
    {
        return await _context.Registrations.Where(r => r.Id == registrationId).ExecuteDeleteAsync() > 0;
    }
    //ADDITIONAL METHODS
    public async Task<bool> SaveChanges()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0;
    }

    public async Task<Registration?> GetRegistrationByUserId(int userId)
    {
        return await _context.Registrations.FirstOrDefaultAsync(r => r.UserId == userId); 
    }

    public async Task<List<Registration>?> GetRegistrationsByRaceId(int raceId)
    {
        return await _context.Registrations.Where(r => r.RaceId == raceId).ToListAsync();
    }

    public async Task<bool> UserRegistered(int userId, int raceId)
    {
        return await _context.Registrations.AnyAsync(r => r.UserId == userId && r.RaceId == raceId);
    }

    public async Task<int> CountRegistrations(int raceId)
    {
        return await _context.Registrations.CountAsync(r => r.RaceId == raceId);
    }
}