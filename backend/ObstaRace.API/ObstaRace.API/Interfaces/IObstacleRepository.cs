using ObstaRace.API.Models;

namespace ObstaRace.API.Interfaces;

public interface IObstacleRepository
{
    ICollection<Obstacle> GetAllObstacle();
    Obstacle GetObstacle(int id);
    bool ObstacleExists(int id);
}