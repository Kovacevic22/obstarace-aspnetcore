using Microsoft.AspNetCore.Mvc;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces.Services;

namespace ObstaRace.API.Controllers;

[Route("api/registrations")]
[ApiController]
public class RegistrationController : ControllerBase
{
    private IRegistrationService _registrationService;
    private ILogger<RegistrationController> _logger;

    public RegistrationController(IRegistrationService registrationService, ILogger<RegistrationController> logger)
    {
        _registrationService = registrationService;
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
            var registrations = await _registrationService.GetAllRegistrations();
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
            var registration = await _registrationService.GetRegistration(id);
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
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateRegistration([FromBody]RegistrationDto registrationDto, [FromQuery]int raceId, [FromQuery]int userId)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            _logger.LogInformation("Creating registration for race {RaceId} and user {UserId}", raceId, userId);
            var registration = await _registrationService.CreateRegistration(raceId, userId, registrationDto.Category);
            return CreatedAtAction(nameof(GetRegistration), new {id = registration.Id}, registration);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error creating registration");
            return StatusCode(500, "Error creating registration");
        }
    }
    [HttpPut("{id:int}")]
    [ProducesResponseType(200, Type = typeof(RegistrationDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateRegistration([FromBody] RegistrationDto registrationDto, int id)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            _logger.LogInformation("Updating registration with id {RegistrationId}", id);
            var registration = await _registrationService.UpdateRegistration(registrationDto, id);
            return Ok(registration);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating registration");
            return StatusCode(500, "Error updating registration");
        }
    }
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteRegistration(int id)
    {
        try
        {
            _logger.LogInformation("Deleting registration with id {RegistrationId}", id);
            await _registrationService.DeleteRegistration(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error deleting registration");
            return StatusCode(500, "Error deleting registration");
        }
    }
    
}