using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces;

namespace ObstaRace.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RaceController : Controller
{
    private readonly IRaceRepository _raceRepository;
    private readonly IMapper _mapper;
    
    public RaceController(IRaceRepository raceRepository, IMapper mapper)
    {
        _raceRepository = raceRepository;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<RaceDto>))]
    public IActionResult GetAllRaces()
    {
        var races = _mapper.Map<List<RaceDto>>(_raceRepository.GetAllRaces());
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(races);
    }

    [HttpGet("{raceId:int}")]
    [ProducesResponseType(200, Type = typeof(RaceDto))]
    [ProducesResponseType(400)]
    public IActionResult GetRaceById(int raceId)
    {
        if(!_raceRepository.RaceExists(raceId))
            return NotFound();
        var race = _mapper.Map<RaceDto>(_raceRepository.GetRace(raceId));
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(race);
    }
}