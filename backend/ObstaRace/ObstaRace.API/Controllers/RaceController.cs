using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.Application.Dto;
using ObstaRace.Application.Interfaces.Services;
using ObstaRace.Domain.Models;


namespace ObstaRace.API.Controllers;

[Route("api/races")]
[ApiController]
public class RaceController : ControllerBase
{
    private readonly IRaceService _raceService;
    private readonly ILogger<RaceController> _logger;
    
    public RaceController(IRaceService raceService, ILogger<RaceController> logger)
    {
        _raceService = raceService;
        _logger = logger;
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<RaceDto>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAllRaces([FromQuery]string? difficulty, [FromQuery]string? distanceRange, 
        [FromQuery]string? search, [FromQuery]int page = 1, [FromQuery]int pageSize = 12)
    {
            if (pageSize > 50) pageSize = 50;
            if (page <= 0) page = 1;
            _logger.LogInformation("Getting races");
            var races = await _raceService.GetAllRaces(difficulty, distanceRange, search,page,pageSize);
            return Ok(races);
    }

    [HttpGet("my-races")]
    [Authorize(Roles = "Organiser")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<RaceDto>))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetMyRaces([FromQuery]int page = 1, int pageSize = 12)
    { 
        if (pageSize > 50) pageSize = 50;
            if (page <= 0) page = 1;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
            _logger.LogInformation("Getting races");
            var races = await _raceService.GetMyRaces(userId, page, pageSize);
            return Ok(races);
    }
    [HttpGet("{raceId:int}")]
    [ProducesResponseType(200, Type = typeof(RaceDto))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetRace(int raceId)
    {
            _logger.LogInformation("Getting race with id {RaceId}", raceId);
            var race = await _raceService.GetRace(raceId);
            
            if (race == null)
            {
                _logger.LogWarning("Race with id {RaceId} not found", raceId);
                return NotFound(new { error = $"Race with id {raceId} not found" });
            }
            
            return Ok(race);
    }

    [HttpGet("{slug}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<RaceDto>))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetRaceBySlug([FromRoute]string slug)
    {
            _logger.LogInformation("Getting race with slug {Slug}", slug);
            var race = await _raceService.GetRaceBySlug(slug);
            if (race == null)
            {
                _logger.LogWarning("Race with slug {Slug} not found", slug);
                return NotFound(new { error = $"Race with slug {slug} not found" });
            }
            return Ok(race);
    }

    [HttpGet("stats")]
    [ProducesResponseType(200, Type = typeof(RaceStatsDto))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetRaceStats()
    {
            _logger.LogInformation("Retrieving dashboard statistics");
            var stats = await _raceService.GetRaceStats();
            return Ok(stats);
    }
    [HttpPost]
    [Authorize(Roles = "Admin,Organiser")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateRace([FromBody]CreateRaceDto raceDto)
    {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
            if(!ModelState.IsValid)
                return BadRequest(new { error = "Invalid data", details = ModelState });
            _logger.LogInformation("Creating race with name {RaceName}", raceDto.Name);
            var race = await _raceService.CreateRace(raceDto,userId);
            return Ok(race);
    }
    [HttpPut("{raceId:int}")]
    [Authorize(Roles = "Admin,Organiser")]
    [ProducesResponseType(200, Type = typeof(RaceDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateRace([FromBody]UpdateRaceDto raceDto,int raceId)
    {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
            var roleClaimValue = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleClaimValue == null) return Unauthorized();
            Role userRole = (Role)Enum.Parse(typeof(Role), roleClaimValue);
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Invalid data", details = ModelState });
            _logger.LogInformation("Updating race with id {RaceId}", raceId);
            var race = await _raceService.UpdateRace(raceDto, raceId,userId,userRole);
            return Ok(race);
    }
    [HttpDelete("{raceId:int}")]
    [Authorize(Roles = "Admin,Organiser")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteRace(int raceId)
    {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
            var roleClaimValue = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleClaimValue == null) return Unauthorized();
            Role userRole = (Role)Enum.Parse(typeof(Role), roleClaimValue);
            _logger.LogInformation("Deleting race with id {RaceId}", raceId);
            await _raceService.DeleteRace(raceId, userId, userRole);
            return NoContent();
    }
}