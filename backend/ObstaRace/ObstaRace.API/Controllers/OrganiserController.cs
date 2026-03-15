using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.Application.Dto;
using ObstaRace.Application.Interfaces.Services;

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
            _logger.LogInformation("Getting pending organisers");
            var organisers = await _organiserService.GetPendingOrganisers();
            return Ok(organisers);
    }

    [HttpPut("verify/{userId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<OrganiserDto>))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> VerifyOrganiser(int userId)
    {

        var result = await _organiserService.VerifyOrganiser(userId);
            if (!result) return BadRequest(new { error = "Could not verify organiser" });
            
            return Ok(new { message = "Organiser verified successfully" });
    }

    [HttpPut("reject/{userId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<OrganiserDto>))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> RejectOrganiser(int userId)
    {
            var result = await _organiserService.RejectOrganiser(userId);
            if (!result) return BadRequest(new { error = "Could not reject organiser" });
            
            return Ok(new { message = "Organiser reject successfully" });
    }
}