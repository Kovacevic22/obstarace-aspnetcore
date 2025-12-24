using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces;

namespace ObstaRace.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ObstacleController : Controller
{
    private readonly IObstacleRepository _obstacleRepository;
    private readonly IMapper _mapper;
    public ObstacleController(IObstacleRepository obstacleRepository, IMapper mapper)
    {
        _obstacleRepository = obstacleRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ObstacleDto>))]
    public IActionResult GetAllObstacle()
    {
        var obstacles = _mapper.Map<List<ObstacleDto>>(_obstacleRepository.GetAllObstacle());
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(obstacles);
    }

    public IActionResult GetObstacle(int obstacleId)
    {
        if (!_obstacleRepository.ObstacleExists(obstacleId))
            return NotFound();
        var obstacle = _mapper.Map<ObstacleDto>(_obstacleRepository.GetObstacle(obstacleId));
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(obstacle);
    }
}