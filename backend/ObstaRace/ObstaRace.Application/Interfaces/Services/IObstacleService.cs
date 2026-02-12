

using ObstaRace.Application.Dto;
using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Interfaces.Services;

public interface IObstacleService
{
    Task<ICollection<ObstacleDto>> GetAllObstacles(int userId, Role role,string? search);
    Task<ObstacleDto?> GetObstacle(int id);
    
    //CRUD
    Task<ObstacleDto> CreateObstacle(CreateObstacleDto obstacle, int userId);
    Task<ObstacleDto> UpdateObstacle(UpdateObstacleDto obstacle, int id,int userId, Role userRole);
    Task<bool> DeleteObstacle(int obstacleId, int userId, Role role);
}