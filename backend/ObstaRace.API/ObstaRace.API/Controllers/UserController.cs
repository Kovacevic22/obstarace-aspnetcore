using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces.Services;

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

    [HttpPut("ban/{userId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> BanUser(int userId)
    {
        try
        {
            _logger.LogInformation("Banning user with id {UserId}", userId);
            var result = await _userService.BanUser(userId);
            if (!result)
            {
                _logger.LogError("User does not exist");
                return NotFound(new { message = "User not found" });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error banning user with id {UserId}", userId);
            return StatusCode(500, new { error = "Error banning user" });
        }
    }
    [HttpPut("unban/{userId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UnbanUser(int userId)
    {
        try
        {
            _logger.LogInformation("Unbanning user with id {UserId}", userId);
            var result = await _userService.UnbanUser(userId);
            if (!result)
            {
                _logger.LogError("User does not exist");
                return NotFound(new { message = "User not found" });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unbanning user with id {UserId}", userId);
            return StatusCode(500, new { error = "Error unbanning user" });
        }
    }
    [HttpGet("stats")]
    [ProducesResponseType(200, Type = typeof(UserStatsDto))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetUserStats()
    {
        try
        {
            _logger.LogInformation("Getting all users stats");
            var users = await _userService.GetUserStats();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error retrieving users stats");
            return StatusCode(500, "Error retrieving users stats");
        }
    }

    [HttpPut("{userId:int}")]
    [Authorize]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateUser(int userId, UpdateParticipantDto updateUserDto)
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized(new { error = "You are not authorized to update this user" });
            int currentUserId = int.Parse(userIdClaim);
            bool isAdmin = User.IsInRole("Admin");
            if (currentUserId != userId && !isAdmin)
            {
                return Unauthorized(new {message = "You are not authorized to update this user"}); 
            }

            var result = await _userService.UpdateUser(updateUserDto, userId);
            if(result==null)return NotFound(new { message = "User not found" });
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error updating user");
            return StatusCode(500, new { error = "Error updating user" });
        }
    }
}