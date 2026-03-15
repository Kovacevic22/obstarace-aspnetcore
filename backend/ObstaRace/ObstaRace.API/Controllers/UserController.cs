using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.Application.Dto;
using ObstaRace.Application.Interfaces.Services;
using ObstaRace.Domain.Models;

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
    public async Task<IActionResult> GetAllUsers([FromQuery]int page = 1, [FromQuery]int pageSize = 12)
    {
            if (pageSize > 50) pageSize = 50;
            if (page <= 0) page = 1;
            _logger.LogInformation("Getting all users");
            var users = await _userService.GetAllUsers(page, pageSize);
            return Ok(users);
    }

    [HttpGet("{userId:int}")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetUserById(int userId)
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

    [HttpPut("ban/{userId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> BanUser(int userId)
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
    [HttpPut("unban/{userId}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UnbanUser(int userId)
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
    [HttpGet("stats")]
    [ProducesResponseType(200, Type = typeof(UserStatsDto))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetUserStats()
    {
            _logger.LogInformation("Getting all users stats");
            var users = await _userService.GetUserStats();
            return Ok(users);
    }

    [HttpPut("{userId:int}")]
    [Authorize]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateUser(int userId, UpdateParticipantDto updateUserDto)
    {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int currentUserId = int.Parse(userIdClaim.Value);
            var roleClaimValue = User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleClaimValue == null) return Unauthorized();
            Role userRole = (Role)Enum.Parse(typeof(Role), roleClaimValue);
            bool isAdmin = userRole==Role.Admin;
            if (currentUserId != userId && !isAdmin)
            {
                return Unauthorized(new {message = "You are not authorized to update this user"}); 
            }

            var result = await _userService.UpdateUser(updateUserDto, userId);
            if(result==null)return NotFound(new { message = "User not found" });
            return Ok(result);
    }
}