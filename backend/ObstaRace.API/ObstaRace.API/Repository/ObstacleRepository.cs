using Microsoft.EntityFrameworkCore;
using ObstaRace.API.Data;
using ObstaRace.API.Interfaces;
using ObstaRace.API.Models;

namespace ObstaRace.API.Repository;

public class ObstacleRepository : IObstacleRepository
{
    private DataContext _context;
    public ObstacleRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<ICollection<Obstacle>> GetAllObstacles()
    {
        return await _context.Obstacles.OrderBy(o => o.Id).ToListAsync();
    }

    public async Task<Obstacle?> GetObstacle(int id)
    {
        return await _context.Obstacles.FindAsync(id);
    }
    public async Task<bool> ObstacleExists(int id)
    {
        return  await _context.Obstacles.AnyAsync(o => o.Id == id);
    }
    //CRUD
    public async Task<bool> CreateObstacle(Obstacle obstacle)
    {
        _context.Obstacles.Add(obstacle);
        return await SaveChanges();
    }
    public async Task<bool> UpdateObstacle(Obstacle obstacle)
    {
        _context.Obstacles.Update(obstacle);
        return await SaveChanges();
    }
    public async Task<bool> DeleteObstacle(int obstacleId)
    {
        int row = await _context.Obstacles.Where(o => o.Id == obstacleId).ExecuteDeleteAsync();
        return row > 0;
    }
    //ADDITIONAL METHODS
    public async Task<bool> SaveChanges()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0;
    }

    public async Task<bool> ObstacleHasRaces(int id)
    {
        return await _context.RaceObstacles.AnyAsync(ro => ro.ObstacleId == id);
    }
}