using Microsoft.EntityFrameworkCore;
using ObstaRace.API.Data;
using ObstaRace.API.Interfaces;
using ObstaRace.API.Models;

namespace ObstaRace.API.Repository;

public class ObstacleRepository : IObstacleRepository
{
    private readonly DataContext _context;
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
}