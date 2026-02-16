using Microsoft.EntityFrameworkCore;
using ObstaRace.Application.Dto;
using ObstaRace.Application.Interfaces.Repositories;
using ObstaRace.Domain.Models;
using ObstaRace.Infrastructure.Data;

namespace ObstaRace.Infrastructure.Repository;

public class RaceRepository : IRaceRepository
{
    private DataContext _context;
    public RaceRepository(DataContext context)
    {
        _context = context;
    }
    //GET
    public async Task<ICollection<Race>> GetAllRaces(string? difficulty, string? distance, string? search, int? page, int? pageSize)
    {
        var query =  _context.Races.AsNoTracking().AsQueryable();
        if(!string.IsNullOrWhiteSpace(search))query = query.Where(r => r.Name.ToLower().Contains(search.ToLower()));
        if (!string.IsNullOrEmpty(difficulty) && difficulty != "all" && int.TryParse(difficulty, out int diffValue)) query = query.Where(r => r.Difficulty == (Difficulty)diffValue);
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
        query = query.OrderByDescending(r => r.CreatedAt);
        if (page.HasValue && pageSize.HasValue)
        {
            query = query
                .Skip((page.Value - 1) * pageSize.Value)
                .Take(pageSize.Value);
        }
        return await query.ToListAsync();
    }

    public async Task<Race?> GetRace(int id)
    {
        return await _context.Races
            .Include(r => r.RaceObstacles)
            .ThenInclude(ro => ro.Obstacle)
            .Include(r => r.Registrations)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Race?> GetRaceBySlug(string slug)
    {
        return await _context.Races
            .Include(r=>r.RaceObstacles)
            .ThenInclude(ro => ro.Obstacle).FirstOrDefaultAsync(r => r.Slug == slug);
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

    public async Task<ICollection<Race>> GetMyRaces(int userId, int? page, int? pageSize)
    {
        var query = _context.Races.AsNoTracking().Where(r => r.CreatedById == userId)
            .Include(r => r.RaceObstacles)
            .ThenInclude(ro => ro.Obstacle)
            .OrderByDescending(r => r.CreatedAt);
        if (page.HasValue && pageSize.HasValue)
        {
            query = (IOrderedQueryable<Race>)query
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
        }
        return await query.ToListAsync();
    }

    //CRUD
    public async Task<bool> CreateRace(Race race)
    {
        await _context.Races.AddAsync(race);
        return await SaveChanges();
    }

    public async Task<bool> UpdateRace(Race race,List<RaceObstacle> toAdd, List<RaceObstacle> toRemove)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            foreach (var ro in toRemove) race.RaceObstacles.Remove(ro);
            foreach (var ro in toAdd) race.RaceObstacles.Add(ro);
        
            _context.Races.Update(race);
            await SaveChanges();
        
            await transaction.CommitAsync();
            return true;
        }
        catch(Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception("Failed to update race with obstacles", ex);
        }
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

    public async Task<List<Race>> GetRacesStartingToday()
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);
        return await _context.Races.Where(r =>
            r.Date.Date >= today &&
            r.Date.Date < tomorrow &&
            r.Status == Status.UpComing).ToListAsync();
    }

    public async Task<List<Race>> GetRacesToComplete()
    {
        return await _context.Races.Where(r =>
            r.Date < DateTime.UtcNow &&       
            r.Status == Status.OnGoing)
            .ToListAsync();
    }

    public async Task<List<Race>> GetCompletedRaces()
    {
        return await _context.Races
            .AsNoTracking()
            .Where(r => r.Status == Status.Completed && r.Date < DateTime.UtcNow)
            .ToListAsync();
    }
    public async IAsyncEnumerable<Registration> StreamRegistrationsForCompletedRace(int raceId)
    {
        await foreach (var registration in _context.Registrations
                           .Include(r => r.User)
                           .ThenInclude(u => u.Participant)
                           .Where(r => 
                               r.RaceId == raceId && 
                               r.Status == RegistrationStatus.Confirmed)
                           .AsAsyncEnumerable())
        {
            yield return registration;
        }
    }

    public async Task<bool> UpdateRaceStatus(int raceId, Status status)
    {
        var race = await _context.Races.FindAsync(raceId);
        if (race == null) return false;
    
        race.Status = status;
        return await SaveChanges();
    }
}