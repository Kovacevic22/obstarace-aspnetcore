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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
            var roleClaimValue = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleClaimValue == null) return Unauthorized();
            Role userRole = (Role)Enum.Parse(typeof(Role), roleClaimValue);
            _logger.LogInformation("User {UserId} with role {Role} requesting obstacles", userId, userRole);
            var obstacles = await _obstacleService.GetAllObstacles(userId,userRole,search);
            return Ok(obstacles);   
    }
    [HttpGet("{obstacleId:int}")]
    [Authorize(Roles = "Admin,Organiser")]
    [ProducesResponseType(200, Type = typeof(ObstacleDto))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetObstacle(int obstacleId)
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
    
    [HttpPost]
    [Authorize(Roles = "Admin,Organiser")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateObstacle([FromBody]CreateObstacleDto obstacleDto)
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
    [HttpPut("{obstacleId:int}")]
    [Authorize(Roles = "Admin,Organiser")]
    [ProducesResponseType(200, Type = typeof(ObstacleDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateObstacle([FromBody]UpdateObstacleDto obstacleDto, int obstacleId)
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
    [HttpDelete("{obstacleId:int}")]
    [Authorize(Roles = "Admin,Organiser")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteObstacle(int obstacleId)
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
}