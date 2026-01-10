using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces.Services;


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
    public async Task<IActionResult> GetAllRaces([FromQuery]string? difficulty, [FromQuery]string? distanceRange, [FromQuery]string? search)
    {
        try
        {
            _logger.LogInformation("Getting races");
            var races = await _raceService.GetAllRaces(difficulty, distanceRange, search);
            return Ok(races);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving races");
            return StatusCode(500, new { error = "Error retrieving races" });
        }
    }

    [HttpGet("{raceId:int}")]
    [ProducesResponseType(200, Type = typeof(RaceDto))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetRace(int raceId)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving race with id {RaceId}", raceId);
            return StatusCode(500, new { error = "Error retrieving race" });
        }
    }

    [HttpGet("{slug}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<RaceDto>))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetRaceBySlug([FromRoute]string slug)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving race");
            return StatusCode(500, new { error = "Error retrieving race" });
        }
    }

    [HttpGet("stats")]
    [ProducesResponseType(200, Type = typeof(RaceStatsDto))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetRaceStats()
    {
        try
        {
            _logger.LogInformation("Retrieving dashboard statistics");
            var stats = await _raceService.GetRaceStats();
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error retrieving race stats");
            return StatusCode(500, new { error = "Error retrieving race stats" });
        }
    }
    [HttpPost]
    [Authorize(Roles = "Admin,Organiser")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateRace([FromBody]CreateRaceDto raceDto)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(new { error = "Invalid data", details = ModelState });
            _logger.LogInformation("Creating race with name {RaceName}", raceDto.Name);
            var race = await _raceService.CreateRace(raceDto);
            return CreatedAtAction(nameof(GetRace), new {raceId = race.Id}, race);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating race");
            return StatusCode(500, new { error = "Error creating race" });
        }
    }
    [HttpPut("{raceId:int}")]
    [Authorize(Roles = "Admin,Organiser")]
    [ProducesResponseType(200, Type = typeof(RaceDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateRace([FromBody]UpdateRaceDto raceDto,int raceId)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { error = "Invalid data", details = ModelState });
            _logger.LogInformation("Updating race with id {RaceId}", raceId);
            var race = await _raceService.UpdateRace(raceDto, raceId);
            return Ok(race);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error updating race");
            return StatusCode(500, new { error = "Error updating race" });
        }
    }
    [HttpDelete("{raceId:int}")]
    [Authorize(Roles = "Admin,Organiser")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteRace(int raceId)
    {
        try
        {
            _logger.LogInformation("Deleting race with id {RaceId}", raceId);
            await _raceService.DeleteRace(raceId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error deleting race");
            return StatusCode(500, new { error = "Error deleting race" });
        }
    }
}