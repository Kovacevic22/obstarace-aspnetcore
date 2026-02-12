using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.Application.Dto;
using ObstaRace.Application.Interfaces.Services;
using ObstaRace.Domain.Models;

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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
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
    [HttpGet("count/{raceId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<RegistrationDto>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CountRegistrations(int raceId)
    {
        try
        {
            _logger.LogInformation("Getting count participants on race {race}",raceId);
            var registrations = await _registrationService.CountRegistrations(raceId);
            return Ok(registrations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving count participants on race");
            return StatusCode(500, new { error = "Error retrieving count participants on race" });
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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int organiserId = int.Parse(userIdClaim.Value);
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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
            var roleClaimValue = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleClaimValue == null) return Unauthorized();
            Role userRole = (Role)Enum.Parse(typeof(Role), roleClaimValue);
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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
            _logger.LogInformation("User {UserId} attempting to delete registration {RegistrationId}", userId, id);
            await _registrationService.DeleteRegistration(id,userId);
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
    [HttpPut("confirm/{registrationId:int}")]
    [Authorize(Roles = "Organiser")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> ConfirmUserRegistration(int registrationId)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int organiserId = int.Parse(userIdClaim.Value);
            var result = await _registrationService.ConfirmUserRegistration(registrationId, organiserId);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("cancel/{registrationId:int}")]
    [Authorize(Roles = "Organiser")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CancelUserRegistration(int registrationId)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int organiserId = int.Parse(userIdClaim.Value);
            var result = await _registrationService.CancelUserRegistration(registrationId, organiserId);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}