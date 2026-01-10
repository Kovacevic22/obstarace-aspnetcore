using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces.Services;

namespace ObstaRace.API.Controllers;
[Route("api/organisers")]
[ApiController]
public class OrganiserController : ControllerBase
{
    private readonly IOrganiserService _organiserService;
    private readonly ILogger<OrganiserController> _logger;

    public OrganiserController(IOrganiserService organiserService, ILogger<OrganiserController> logger)
    {
        _organiserService = organiserService;
        _logger = logger;
    }

    [HttpGet("pending")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<OrganiserDto>))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetPendingOrganisers()
    {
        try
        {
            _logger.LogInformation("Getting pending organisers");
            var organisers = await _organiserService.GetPendingOrganisers();
            return Ok(organisers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving pending organisers");
            return StatusCode(500, new { error = "Error retrieving pending organisers" });
        }
    }

    [HttpPut("verify/{userId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<OrganiserDto>))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> VerifyOrganiser(int userId)
    {
        try
        {
            var result = await _organiserService.VerifyOrganiser(userId);
            if (!result) return BadRequest(new { error = "Could not verify organiser" });
            
            return Ok(new { message = "Organiser verified successfully" });
        }
        catch (ArgumentException ax)
        {
            _logger.LogError(ax, "Error verifying pending organisers");
            return BadRequest(new { error = ax.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying pending organisers");
            return StatusCode(500, new { error = "Error verifying pending organisers" });
        }
    }
}