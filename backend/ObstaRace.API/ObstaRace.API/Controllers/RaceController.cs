using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces;

namespace ObstaRace.API.Controllers;

[Route("api/races")]
[ApiController]
public class RaceController : ControllerBase
{
    private readonly IRaceRepository _raceRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<RaceController> _logger;
    
    public RaceController(IRaceRepository raceRepository, IMapper mapper, ILogger<RaceController> logger)
    {
        _raceRepository = raceRepository;
        _mapper = mapper;
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
            var races = _mapper.Map<List<RaceDto>>(await _raceRepository.GetAllRaces());
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
            var race = _mapper.Map<RaceDto>(await _raceRepository.GetRace(raceId));
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
}