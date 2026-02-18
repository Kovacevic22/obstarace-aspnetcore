

using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Interfaces.Repositories;

public interface IObstacleRepository
{
    Task<ICollection<Obstacle>> GetAllObstacles(string? search);
    Task<Obstacle?> GetObstacle(int id);
    Task<ICollection<Obstacle>> GetObstaclesFromCreator(int userId,string? search);
    //CRUD
    Task<bool> CreateObstacle(Obstacle obstacle);
    Task<bool> UpdateObstacle(Obstacle obstacle);
    Task<bool> DeleteObstacle(int obstacleId);
    //ADDITIONAL METHODS
    Task<bool> SaveChanges();
    Task<bool> ObstacleHasRaces(int id);
}