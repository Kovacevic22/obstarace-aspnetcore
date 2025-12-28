using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces;
using ObstaRace.API.Interfaces.Services;
using ObstaRace.API.Models;

namespace ObstaRace.API.Controllers;
[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;
    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            _logger.LogInformation("Getting all users");
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return StatusCode(500, new { error = "Error retrieving users" });
        }
    }

    [HttpGet("{userId:int}")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetUserById(int userId)
    {
        try
        {
            _logger.LogInformation("Getting user with id {UserId}", userId);
            var user = await _userService.GetUser(userId);
            if (user == null)
            {
                _logger.LogWarning("User with id {UserId} not found", userId);
                return NotFound(new { error = $"User with id {userId} not found" });
            }
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with id {UserId}", userId);
            return StatusCode(500, new { error = "Error retrieving user" });
        }
    }
}