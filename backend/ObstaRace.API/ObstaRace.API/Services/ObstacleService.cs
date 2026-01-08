using AutoMapper;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces;
using ObstaRace.API.Interfaces.Services;
using ObstaRace.API.Models;

namespace ObstaRace.API.Services;

public class ObstacleService : IObstacleService
{
    private readonly ILogger<ObstacleService> _logger;
    private readonly IMapper _mapper;
    private readonly IObstacleRepository _obstacleRepository;
    public ObstacleService(IObstacleRepository obstacleRepository, IMapper mapper, ILogger<ObstacleService> logger)
    {
        _obstacleRepository = obstacleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<ObstacleDto>> GetAllObstacles(int userId, Role role, string? search)
    {
        ICollection<Obstacle> obstacles;
        if (role == Role.Admin)
        {
             obstacles = await _obstacleRepository.GetAllObstacles(search);
        }
        else
        {
            obstacles = await _obstacleRepository.GetObstaclesFromCreator(userId,search);
        }
        return _mapper.Map<List<ObstacleDto>>(obstacles);
    }

    public async Task<ObstacleDto?> GetObstacle(int id)
    {
        var obstacle = await _obstacleRepository.GetObstacle(id);
        return obstacle==null?null:_mapper.Map<ObstacleDto>(obstacle);
    }
    public async Task<ObstacleDto> CreateObstacle(CreateObstacleDto obstacleDto, int userId)
    {
        _logger.LogInformation("Creating obstacle {ObstacleDto.Name}", obstacleDto.Name);
        var obstacle = _mapper.Map<Obstacle>(obstacleDto);
        obstacle.CreatedById = userId;
        if (!await _obstacleRepository.CreateObstacle(obstacle))
        {
            _logger.LogError("Failed to create obstacle in database");
            throw new Exception("Failed to create obstacle in database");
        }
        return _mapper.Map<ObstacleDto>(obstacle);
    }
    public async Task<ObstacleDto> UpdateObstacle(UpdateObstacleDto obstacle, int id)
    {
        _logger.LogInformation("Updating obstacle {ObstacleDto.Name}", obstacle.Name);
        var existingObstacle = await _obstacleRepository.GetObstacle(id);
        if (existingObstacle == null)
        {
            _logger.LogWarning("Obstacle with id {id} does not exist", id);
            throw new ArgumentException($"Obstacle with id {id} does not exist");
        }
        existingObstacle.Name = obstacle.Name;
        existingObstacle.Description = obstacle.Description;
        existingObstacle.Difficulty = obstacle.Difficulty;
        if (!await _obstacleRepository.UpdateObstacle(existingObstacle))
        {
            _logger.LogError("Failed to update obstacle in database");
            throw new Exception("Failed to update obstacle in database");
        }
        
        return _mapper.Map<ObstacleDto>(existingObstacle);
    }

    public async Task<bool> DeleteObstacle(int obstacleId, int userId, Role role)
    {
        var obstacle = await _obstacleRepository.GetObstacle(obstacleId);
        _logger.LogInformation("Delete obstacle {obstacleId}",obstacleId);
        if (role != Role.Admin && obstacle?.CreatedById != userId)
        {
            _logger.LogError("Unauthorized deleting obstacle");
            throw new UnauthorizedAccessException("You can only delete your own obstacles.");
        }
        if (await _obstacleRepository.ObstacleHasRaces(obstacleId))
        {
            _logger.LogInformation("Obstacle has race");
            throw new ArgumentException("Obstacle has race");
        }
        return await _obstacleRepository.DeleteObstacle(obstacleId);
    }
}