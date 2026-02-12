using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.Application.Dto;
using ObstaRace.Application.Interfaces.Services;
using ObstaRace.Domain.Models;

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
    [Authorize(Roles = "Admin,Organiser")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ObstacleDto>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAllObstacle(string? search)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
            var roleClaimValue = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleClaimValue == null) return Unauthorized();
            Role userRole = (Role)Enum.Parse(typeof(Role), roleClaimValue);
            _logger.LogInformation("User {UserId} with role {Role} requesting obstacles", userId, userRole);
            var obstacles = await _obstacleService.GetAllObstacles(userId,userRole,search);
            return Ok(obstacles);   
        }catch(Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all obstacles");
            return StatusCode(500, new { error = "Error retrieving obstacles" });
        }
    }
    [HttpGet("{obstacleId:int}")]
    [Authorize(Roles = "Admin,Organiser")]
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
    [Authorize(Roles = "Admin,Organiser")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateObstacle([FromBody]CreateObstacleDto obstacleDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Invalid data", details = ModelState });
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
            _logger.LogInformation("Creating obstacle {ObstacleDto.Name}", obstacleDto.Name);
            var obstacle = await _obstacleService.CreateObstacle(obstacleDto,userId);
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
    [Authorize(Roles = "Admin,Organiser")]
    [ProducesResponseType(200, Type = typeof(ObstacleDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateObstacle([FromBody]UpdateObstacleDto obstacleDto, int obstacleId)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
            var roleClaimValue = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleClaimValue == null) return Unauthorized();
            Role userRole = (Role)Enum.Parse(typeof(Role), roleClaimValue);
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Invalid data", details = ModelState });
            _logger.LogInformation("Updating obstacle {obstacleId}", obstacleId);
            var obstacle = await _obstacleService.UpdateObstacle(obstacleDto, obstacleId,userId, userRole);
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
    [Authorize(Roles = "Admin,Organiser")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteObstacle(int obstacleId)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
            var roleClaimValue = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleClaimValue == null) return Unauthorized();
            Role userRole = (Role)Enum.Parse(typeof(Role), roleClaimValue);
            _logger.LogInformation("User with id {userId} and role {userRole} deleting obstacle {idObstacle}", userId,
                userRole, obstacleId);
            var result = await _obstacleService.DeleteObstacle(obstacleId, userId, userRole);
            if (!result)
            {
                _logger.LogWarning("Failed to delete obstacle {obstacleId}", obstacleId);
                return StatusCode(500, new { error = "Failed to delete obstacle" });
            }

            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return  Unauthorized(new { error = ex.Message });
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