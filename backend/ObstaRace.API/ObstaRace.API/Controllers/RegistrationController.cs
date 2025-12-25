using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces;

namespace ObstaRace.API.Controllers;

[Route("api/registrations")]
[ApiController]
public class RegistrationController : ControllerBase
{
    private readonly IRegistrationRepository _registrationRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<RegistrationController> _logger;

    public RegistrationController(IRegistrationRepository registrationRepository, IMapper mapper, ILogger<RegistrationController> logger)
    {
        _registrationRepository = registrationRepository;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<RegistrationDto>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAllRegistrations()
    {
        try
        {
            _logger.LogInformation("Getting all registrations");
            var registrations = _mapper.Map<List<RegistrationDto>>(await _registrationRepository.GetAllRegistrations());
            return Ok(registrations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving registrations");
            return StatusCode(500, "Error retrieving registrations");
        }
    }
    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(RegistrationDto))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetRegistration(int id)
    {
        try
        {
            _logger.LogInformation("Getting registration with id {RegistrationId}", id);
            var registration = _mapper.Map<RegistrationDto>(await _registrationRepository.GetRegistration(id));
            if (registration == null)
            {
                _logger.LogWarning("Registration with id {RegistrationId} not found", id);
                return NotFound($"Registration with id {id} not found");
            }

            return Ok(registration);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving registration with id {RegistrationId}", id);
            return StatusCode(500, "Error retrieving registration");
        }
    }
}