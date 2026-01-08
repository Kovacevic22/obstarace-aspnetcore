using Microsoft.EntityFrameworkCore;
using ObstaRace.API.Data;
using ObstaRace.API.Dto;
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
    public async Task<ICollection<Race>> GetAllRaces(string? difficulty, string? distance, string? search)
    {
        var query =  _context.Races.AsQueryable();
        if(!string.IsNullOrWhiteSpace(search))query = query.Where(r => r.Name.ToLower().Contains(search.ToLower()));
        if (!string.IsNullOrEmpty(difficulty) && difficulty != "all") query = query.Where(r => r.Difficulty == (Difficulty)int.Parse(difficulty));
        if (!string.IsNullOrEmpty(distance) && distance != "all" && distance != "Any distance")
        {
            query = distance switch
            {
                "5" => query.Where(r => r.Distance <= 5),
                "15" => query.Where(r => r.Distance > 5 && r.Distance <= 15),
                "1000" => query.Where(r => r.Distance > 15),
                _ => query
            };
        }

        return await query.OrderBy(r => r.Id).ToListAsync();
    }

    public async Task<Race?> GetRace(int id)
    {
        return await _context.Races
            .Include(r => r.RaceObstacles)
            .Include(r => r.Registrations)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Race?> GetRaceBySlug(string slug)
    {
        return await _context.Races.FirstOrDefaultAsync(r => r.Slug == slug);
    }

    public async Task<RaceStatsDto> GetRaceStats()
    {
        return new RaceStatsDto()
        {
            TotalRaces =  await _context.Races.CountAsync(),
            ArchivedCount = await _context.Races.CountAsync(r => r.Status == Status.Completed),
            TotalKilometers = await _context.Races.SumAsync(r => r.Distance)
        };
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