using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces.Services;

namespace ObstaRace.API.Controllers;
[Route("api/obstacles")]
[ApiController]
public class ObstacleController : ControllerBase
{
    private readonly IObstacleService _obstacleService;
    private readonly ILogger<ObstacleController> _logger;
    public ObstacleController(IObstacleService obstacleService, ILogger<ObstacleController> logger)
    {
        _obstacleService = obstacleService;
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
            var obstacles = await _obstacleService.GetAllObstacles();
            return Ok(obstacles);   
        }catch(Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all obstacles");
            return StatusCode(500, new { error = "Error retrieving obstacles" });
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
            var obstacle = await _obstacleService.GetObstacle(obstacleId);
            if (obstacle == null)
            {
                _logger.LogWarning("Obstacle with id {ObstacleId} not found", obstacleId);
                return NotFound(new { error = $"Obstacle with id {obstacleId} not found" });
            }
            return Ok(obstacle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving obstacle with id {ObstacleId}", obstacleId);
            return StatusCode(500, new { error = "Error retrieving obstacle" });
        }
    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateObstacle([FromBody]CreateObstacleDto obstacleDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Invalid data", details = ModelState });
            _logger.LogInformation("Creating obstacle {ObstacleDto.Name}", obstacleDto.Name);
            var obstacle = await _obstacleService.CreateObstacle(obstacleDto);
            return CreatedAtAction(nameof(GetObstacle), new { obstacleId = obstacle.Id }, obstacle);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error creating obstacle");
            return StatusCode(500, new { error = "Error creating obstacle" });
        }
    }
    [HttpPut("{obstacleId:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200, Type = typeof(ObstacleDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateObstacle([FromBody]UpdateObstacleDto obstacleDto, int obstacleId)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Invalid data", details = ModelState });
            _logger.LogInformation("Updating obstacle {obstacleId}", obstacleId);
            var obstacle = await _obstacleService.UpdateObstacle(obstacleDto, obstacleId);
            return Ok(obstacle);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error updating obstacle");
            return StatusCode(500, new { error = "Error updating obstacle" });
        }
    }
    [HttpDelete("{obstacleId:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteObstacle(int obstacleId)
    {
        try
        {
            _logger.LogInformation("Deleting obstacle {idObstacle}", obstacleId);
            var result = await _obstacleService.DeleteObstacle(obstacleId);
            if (!result)
            {
                _logger.LogWarning("Failed to delete obstacle {obstacleId}", obstacleId);
                return StatusCode(500, new { error = "Failed to delete obstacle" });
            }

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error deleting obstacle");
            return StatusCode(500, new { error = "Error deleting obstacle" });
        }
    }
}