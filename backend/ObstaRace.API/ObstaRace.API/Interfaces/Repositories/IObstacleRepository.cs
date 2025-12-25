using ObstaRace.API.Models;

namespace ObstaRace.API.Interfaces;

public interface IObstacleRepository
{
    Task<ICollection<Obstacle>> GetAllObstacles();
    Task<Obstacle?> GetObstacle(int id);
    Task<bool> ObstacleExists(int id);
    
    //CRUD
    Task<bool> CreateObstacle(Obstacle obstacle);
    Task<bool> UpdateObstacle(Obstacle obstacle);
    Task<bool> DeleteObstacle(int obstacleId);
    //ADDITIONAL METHODS
    Task<bool> SaveChanges();
    Task<bool> ObstacleHasRaces(int id);
}