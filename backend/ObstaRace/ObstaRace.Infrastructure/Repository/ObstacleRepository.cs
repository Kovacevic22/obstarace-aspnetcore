using Microsoft.EntityFrameworkCore;
using ObstaRace.Application.Interfaces.Repositories;
using ObstaRace.Domain.Models;
using ObstaRace.Infrastructure.Data;

namespace ObstaRace.Infrastructure.Repository;

public class ObstacleRepository : IObstacleRepository
{
    private DataContext _context;
    public ObstacleRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<ICollection<Obstacle>> GetAllObstacles(string? search)
    {
        if (string.IsNullOrEmpty(search)) return await _context.Obstacles.AsNoTracking().OrderByDescending(o => o.CreatedAt).ToListAsync();
        return await _context.Obstacles.AsNoTracking().Where(o => o.Name.ToLower().Contains(search.ToLower())).OrderByDescending(o=>o.CreatedAt).ToListAsync();
    }

    public async Task<Obstacle?> GetObstacle(int id)
    {
        return await _context.Obstacles.FindAsync(id);
    }
    public async Task<bool> ObstacleExists(int id)
    {
        return  await _context.Obstacles.AnyAsync(o => o.Id == id);
    }

    public async Task<ICollection<Obstacle>> GetObstaclesFromCreator(int userId, string? search)
    {
        if(string.IsNullOrEmpty(search))return await _context.Obstacles.AsNoTracking().Where(o => o.CreatedById==userId).OrderBy(o => o.Id).ToListAsync();
        return await _context.Obstacles.AsNoTracking().Where(o => o.CreatedById == userId && o.Name.ToLower().Contains(search.ToLower())).OrderBy(o => o.CreatedAt).ToListAsync();
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