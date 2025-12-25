using Microsoft.AspNetCore.Mvc;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces.Services;


namespace ObstaRace.API.Controllers;

[Route("api/races")]
[ApiController]
public class RaceController : ControllerBase
{
    private IRaceService _raceService;
    private ILogger<RaceController> _logger;
    
    public RaceController(IRaceService raceService, ILogger<RaceController> logger)
    {
        _raceService = raceService;
        _logger = logger;
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<RaceDto>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAllRaces()
    {
        try
        {
            _logger.LogInformation("Getting all races");
            var races = await _raceService.GetAllRaces();
            return Ok(races);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving races");
            return StatusCode(500, "Error retrieving races");
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
                return NotFound($"Race with id {raceId} not found");
            }
            
            return Ok(race);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving race with id {RaceId}", raceId);
            return StatusCode(500, "Error retrieving race");
        }
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateRace([FromBody]RaceDto raceDto)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            _logger.LogInformation("Creating race with name {RaceName}", raceDto.Name);
            var race = await _raceService.CreateRace(raceDto);
            return CreatedAtAction(nameof(GetRace), new {raceId = race.Id}, race);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error creating race");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating race");
            return StatusCode(500, "Error creating race");
        }
    }
    [HttpPut("{raceId:int}")]
    [ProducesResponseType(200, Type = typeof(RaceDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateRace([FromBody]RaceDto raceDto,int raceId)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _logger.LogInformation("Updating race with id {RaceId}", raceId);
            var race = await _raceService.UpdateRace(raceDto, raceId);
            return Ok(race);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error updating race");
            return StatusCode(500, "Error updating race");
        }
    }
    [HttpDelete("{raceId:int}")]
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
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error deleting race");
            return StatusCode(500, "Error deleting race");
        }
    }
}