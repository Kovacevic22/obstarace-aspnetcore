using Microsoft.EntityFrameworkCore;
using ObstaRace.API.Data;
using ObstaRace.API.Interfaces;
using ObstaRace.API.Models;

namespace ObstaRace.API.Repository;

public class RaceRepository : IRaceRepository
{
    private readonly DataContext _context;
    public RaceRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<ICollection<Race>> GetAllRaces()
    {
        return await _context.Races.OrderBy(r => r.Id).ToListAsync();
    }

    public async Task<Race?> GetRace(int id)
    {
        return await _context.Races.FindAsync(id);
    }

    public async Task<bool> RaceExists(int id)
    {
        return await _context.Races.AnyAsync(r => r.Id == id);
    }
}