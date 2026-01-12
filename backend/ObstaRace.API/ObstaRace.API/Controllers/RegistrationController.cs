using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces.Services;
using ObstaRace.API.Models;

namespace ObstaRace.API.Controllers;

[Route("api/registrations")]
[ApiController]
public class RegistrationController : ControllerBase
{
    private readonly IRegistrationService _registrationService;
    private readonly ILogger<RegistrationController> _logger;

    public RegistrationController(IRegistrationService registrationService, ILogger<RegistrationController> logger)
    {
        _registrationService = registrationService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(200, Type = typeof(IEnumerable<RegistrationDto>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAllRegistrations()
    {
        try
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            _logger.LogInformation("Getting all registrations");
            var registrations = await _registrationService.GetAllRegistrations(userId);
            return Ok(registrations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving registrations");
            return StatusCode(500, new { error = "Error retrieving registrations" });
        }
    }
    [HttpGet("{id:int}")]
    [Authorize]
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
                return NotFound(new { error = $"Registration with id {id} not found" });
            }
            return Ok(registration);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving registration");
            return StatusCode(500, new { error = "Error retrieving registration" });
        }
    }

    [HttpGet("registrations-on-race")]
    [Authorize(Roles = "Organiser")]
    [ProducesResponseType(200, Type = typeof(RegistrationDto))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetParticipantsForRace([FromQuery]int? raceId)
    {
        try
        {
            var organiserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            _logger.LogInformation("Organiser {organiserId} requesting participants for race {RaceId}", organiserId, raceId);
            var participants = await _registrationService.GetParticipantsForRace(organiserId, raceId);
            return Ok(participants);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching participants for race {RaceId}", raceId);
            return StatusCode(500, new { error = "Internal server error" });
        }
    }
    [HttpPost]
    [Authorize]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateRegistration([FromBody] CreateRegistrationDto registrationDto)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if(!ModelState.IsValid)
                return BadRequest(new { error = "Invalid data", details = ModelState });
            _logger.LogInformation("Creating registration for race {RaceId} and user {UserId}", registrationDto.RaceId, userId);
            var registration = await _registrationService.CreateRegistration(registrationDto.RaceId,userId);
            return CreatedAtAction(nameof(GetRegistration), new {id = registration.Id}, registration);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error creating registration");
            return StatusCode(500, new { error = "Error creating registration" });
        }
    }
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Organiser")]
    [ProducesResponseType(200, Type = typeof(RegistrationDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateRegistration([FromBody] UpdateRegistrationDto registrationDto, int id)
    {
        try
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var userRole = (Role)Enum.Parse(typeof(Role), User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value);
            if(!ModelState.IsValid)
                return BadRequest(new { error = "Invalid data", details = ModelState });
            _logger.LogInformation("Updating registration with id {RegistrationId}", id);
            var registration = await _registrationService.UpdateRegistration(registrationDto, id,userId,userRole);
            return Ok(registration);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating registration");
            return StatusCode(500, new { error = "Error updating registration" });
        }
    }
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "User,Organiser")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteRegistration(int id)
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if(userIdClaim == null)return Unauthorized(new {message = "You are not authorized to delete this registration"});
            int currentUserId = int.Parse(userIdClaim.Value);
            _logger.LogInformation("User {UserId} attempting to delete registration {RegistrationId}", currentUserId, id);
            await _registrationService.DeleteRegistration(id,currentUserId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error deleting registration");
            return StatusCode(500, new { error = "Error deleting registration" });
        }
    }
    
}