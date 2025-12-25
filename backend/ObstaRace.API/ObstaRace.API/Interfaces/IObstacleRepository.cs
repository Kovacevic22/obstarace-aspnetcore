using ObstaRace.API.Models;

namespace ObstaRace.API.Interfaces;

public interface IObstacleRepository
{
    Task<ICollection<Obstacle>> GetAllObstacles();
    Task<Obstacle?> GetObstacle(int id);
    Task<bool> ObstacleExists(int id);
}