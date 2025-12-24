using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces;

namespace ObstaRace.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegistrationController : Controller
{
    private readonly IRegistrationRepository _registrationRepository;
    private readonly IMapper _mapper;
    public RegistrationController(IRegistrationRepository registrationRepository, IMapper mapper)
    {
        _registrationRepository = registrationRepository;
        _mapper = mapper;
    }
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<RegistrationDto>))]
    public IActionResult GetAllRegistrations()
    {
        var registrations = _mapper.Map<List<RegistrationDto>>(_registrationRepository.GetAllRegistrations());
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(registrations);
    }
    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(RegistrationDto))]
    [ProducesResponseType(400)]
    public IActionResult GetRegistration(int id)
    {
        if (!_registrationRepository.RegistrationExists(id))
            return NotFound();
        var registration = _mapper.Map<RegistrationDto>(_registrationRepository.GetRegistration(id));
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(registration);
    }
}