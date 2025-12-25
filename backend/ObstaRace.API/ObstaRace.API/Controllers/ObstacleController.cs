using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces;

namespace ObstaRace.API.Controllers;
[Route("api/obstacles")]
[ApiController]
public class ObstacleController : ControllerBase
{
    private readonly IObstacleRepository _obstacleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ObstacleController> _logger;
    public ObstacleController(IObstacleRepository obstacleRepository, IMapper mapper, ILogger<ObstacleController> logger)
    {
        _obstacleRepository = obstacleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ObstacleDto>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAllObstacles()
    {
        try
        {
            _logger.LogInformation("Getting all obstacles");
            var obstacles = _mapper.Map<List<ObstacleDto>>(await _obstacleRepository.GetAllObstacles());
            return Ok(obstacles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error retrieving obstacles");
            return StatusCode(500, "Error retrieving obstacles");
        }
    }
    [HttpGet("{obstacleId:int}")]
    [ProducesResponseType(200, Type = typeof(ObstacleDto))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetObstacle(int obstacleId)
    {
        try
        {
            _logger.LogInformation("Getting obstacle with id {ObstacleId}", obstacleId);
            var obstacle = _mapper.Map<ObstacleDto>(await _obstacleRepository.GetObstacle(obstacleId));
            if (obstacle == null)
            {
                _logger.LogWarning("Obstacle with id {ObstacleId} not found", obstacleId);
                return NotFound($"Obstacle with id {obstacleId} not found");
            }
            return Ok(obstacle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving obstacle with id {ObstacleId}", obstacleId);
            return StatusCode(500, "Error retrieving obstacle");
        }
    }
}