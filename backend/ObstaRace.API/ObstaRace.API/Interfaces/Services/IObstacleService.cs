using ObstaRace.API.Dto;

namespace ObstaRace.API.Interfaces.Services;

public interface IObstacleService
{
    Task<ICollection<ObstacleDto>> GetAllObstacles();
    Task<ObstacleDto?> GetObstacle(int id);
    
    //CRUD
    Task<ObstacleDto> CreateObstacle(ObstacleDto obstacle);
    Task<ObstacleDto> UpdateObstacle(ObstacleDto obstacle, int id);
    Task<bool> DeleteObstacle(int obstacleId);
}