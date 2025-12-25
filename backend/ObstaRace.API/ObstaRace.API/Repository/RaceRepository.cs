using Microsoft.EntityFrameworkCore;
using ObstaRace.API.Data;
using ObstaRace.API.Interfaces;
using ObstaRace.API.Models;

namespace ObstaRace.API.Repository;

public class RaceRepository : IRaceRepository
{
    private DataContext _context;
    public RaceRepository(DataContext context)
    {
        _context = context;
    }
    //GET
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
    //CRUD
    public async Task<bool> CreateRace(Race race)
    {
        await _context.Races.AddAsync(race);
        return await SaveChanges();
    }

    public async Task<bool> UpdateRace(Race race)
    {
        _context.Races.Update(race);
        return await SaveChanges();
    }

    public async Task<bool> DeleteRace(int raceId)
    {
        return await _context.Races.Where(r => r.Id == raceId).ExecuteDeleteAsync() > 0;
    }
    //ADDITIONAL METHODS
    public async Task<bool> SaveChanges()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0;
    }
    public async Task<bool> RaceNameExists(string name)
    {
        return await _context.Races.AnyAsync(r => r.Name == name);
    }
    public async Task<bool> RaceHasRegistrations(int id)
    {
        return await _context.Registrations.AnyAsync(r => r.RaceId == id);
    }

    public async Task<bool> RaceHasObstacles(int id)
    {
        return await _context.RaceObstacles.AnyAsync(r => r.RaceId == id);
    }
}