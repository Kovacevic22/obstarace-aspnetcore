using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces.Services;

namespace ObstaRace.API.Controllers.Auth;
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<AuthController> _logger;
    public AuthController(IUserService userService, ILogger<AuthController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("register")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            var user = await _userService.RegisterUser(registerDto);
            if (user == null)
            {
                _logger.LogError("Error registering user");
                return BadRequest(new { error = "Error registering user" });
            }
            return Ok(user);
        }
        catch (ArgumentException ax)
        {
            return BadRequest(new { error = ax.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error registering user");
            return StatusCode(500, new { error = "Error registering user" });
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(200, Type = typeof(LoginResponseDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            _logger.LogInformation("Logging in user");
            var response = await _userService.LoginUser(loginDto);
            if (response == null)
            {
                _logger.LogError("Error logging in user");
                return Unauthorized(new { error = "Invalid email or password" });
            }
            Response.Cookies.Append("X-Access-Token", response.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });
            return Ok(new {user = response.User});
        }
        catch (ArgumentException ax)
        {
            return Unauthorized(new { error = ax.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error logging in user");
            return StatusCode(500, new { error = "Error logging in user" });
        }
    }

    [HttpPost("logout")]
    public  IActionResult Logout()
    {
        Response.Cookies.Append("X-Access-Token", "",new CookieOptions
        {
            HttpOnly =  true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(-1)
        });
        return Ok(new { message = "Logged out successfully" });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
        var userIdClaim = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return Unauthorized();
        int userId = int.Parse(userIdClaim);
        var user = await _userService.GetUser(userId);
        if (user == null) return NotFound();
        return Ok(user);
    }
}