using ObstaRace.API.Data;
using ObstaRace.API.Models;

namespace ObstaRace.API.Repository;

public class ObstacleRepository
{
    private readonly DataContext _context;
    public ObstacleRepository(DataContext context)
    {
        _context = context;
    }
    public ICollection<Obstacle> GetAllObstacle()
    {
        return _context.Obstacles.OrderBy(o => o.Id).ToList();
    }

    public Obstacle GetObstacle(int id)
    {
        return _context.Obstacles.Where(o => o.Id == id).FirstOrDefault();
    }
    public bool ObstacleExists(int id)
    {
        return _context.Obstacles.Any(o => o.Id == id);
    }
}