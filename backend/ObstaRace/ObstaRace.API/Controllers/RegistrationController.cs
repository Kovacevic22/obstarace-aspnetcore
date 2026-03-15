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
    public async Task<IActionResult> GetAllRegistrations([FromQuery]int page = 1, [FromQuery]int pageSize = 12)
    {
            if (pageSize > 50) pageSize = 50;
            if (page <= 0) page = 1;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
            _logger.LogInformation("Getting all registrations");
            var registrations = await _registrationService.GetAllRegistrations(userId,page,pageSize);
            return Ok(registrations);
    }
    [HttpGet("count/{raceId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<RegistrationDto>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CountRegistrations(int raceId)
    {
            _logger.LogInformation("Getting count participants on race {race}",raceId);
            var registrations = await _registrationService.CountRegistrations(raceId);
            return Ok(registrations);
    }
    [HttpGet("{id:int}")]
    [Authorize]
    [ProducesResponseType(200, Type = typeof(RegistrationDto))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetRegistration(int id)
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

    [HttpGet("registrations-on-race")]
    [Authorize(Roles = "Organiser")]
    [ProducesResponseType(200, Type = typeof(RegistrationDto))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetParticipantsForRace([FromQuery]int? raceId,[FromQuery]int page=1, [FromQuery]int pageSize=12)
    {
            if (pageSize > 50) pageSize = 50;
            if (page <= 0) page = 1;
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int organiserId = int.Parse(userIdClaim.Value);
            _logger.LogInformation("Organiser {organiserId} requesting participants for race {RaceId}", organiserId, raceId);
            var participants = await _registrationService.GetParticipantsForRace(organiserId, raceId, page, pageSize);
            return Ok(participants);
    }
    [HttpPost]
    [Authorize]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateRegistration([FromBody] CreateRegistrationDto registrationDto)
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
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Organiser")]
    [ProducesResponseType(200, Type = typeof(RegistrationDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateRegistration([FromBody] UpdateRegistrationDto registrationDto, int id)
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
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "User,Organiser")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteRegistration(int id)
    {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
            _logger.LogInformation("User {UserId} attempting to delete registration {RegistrationId}", userId, id);
            await _registrationService.DeleteRegistration(id,userId);
            return NoContent();
    }
    [HttpPut("confirm/{registrationId:int}")]
    [Authorize(Roles = "Organiser")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> ConfirmUserRegistration(int registrationId)
    {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int organiserId = int.Parse(userIdClaim.Value);
            var result = await _registrationService.ConfirmUserRegistration(registrationId, organiserId);
            return Ok(result);
    }

    [HttpPut("cancel/{registrationId:int}")]
    [Authorize(Roles = "Organiser")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CancelUserRegistration(int registrationId)
    {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int organiserId = int.Parse(userIdClaim.Value);
            var result = await _registrationService.CancelUserRegistration(registrationId, organiserId);
            return Ok(result);
    }
    [HttpGet("check/{raceId:int}")]
    [Authorize] 
    public async Task<IActionResult> IsUserRegistered(int raceId)
    {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
            bool isRegistered = await _registrationService.IsUserRegistered(raceId, userId);
            return Ok(isRegistered); 
    }
}