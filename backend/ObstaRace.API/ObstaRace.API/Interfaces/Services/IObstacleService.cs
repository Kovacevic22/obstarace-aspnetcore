using ObstaRace.API.Dto;

namespace ObstaRace.API.Interfaces.Services;

public interface IObstacleService
{
    Task<ICollection<ObstacleDto>> GetAllObstacles();
    Task<ObstacleDto?> GetObstacle(int id);
    
    //CRUD
    Task<ObstacleDto> CreateObstacle(CreateObstacleDto obstacle);
    Task<ObstacleDto> UpdateObstacle(UpdateObstacleDto obstacle, int id);
    Task<bool> DeleteObstacle(int obstacleId);
}