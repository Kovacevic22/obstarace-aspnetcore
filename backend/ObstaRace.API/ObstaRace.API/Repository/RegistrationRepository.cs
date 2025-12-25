using Microsoft.EntityFrameworkCore;
using ObstaRace.API.Data;
using ObstaRace.API.Interfaces;
using ObstaRace.API.Models;

namespace ObstaRace.API.Repository;

public class RegistrationRepository : IRegistrationRepository
{
    private readonly DataContext _context;
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
}