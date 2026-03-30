using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.Application.Dto;
using ObstaRace.Application.Interfaces.Services;
using ObstaRace.Application.Interfaces.Services.Auth;

namespace ObstaRace.API.Controllers.Auth;
[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _config;
    public AuthController(IAuthService authService,IUserService userService , ILogger<AuthController> logger, IConfiguration config)
    {
        _authService = authService;
        _userService = userService;
        _logger = logger;
        _config = config;
    }
    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string token)
    {
        var result = await _authService.VerifyEmail(token);
        var frontendUrl = _config["FrontendSettings:BaseUrl"];
        if (!result) return Redirect($"{frontendUrl}/verify-email?success=false");
        return Redirect($"{frontendUrl}/verify-email?success=true");
    }
    [HttpPost("resend-verification")]
    public async Task<IActionResult> ResendVerification([FromBody] ResendVerificationDto dto)
    {
        var result = await _authService.ResendVerificationEmail(dto.Email);
        if (!result) return BadRequest(new { error = "User not found or already verified" });
        return Ok(new { message = "Verification email sent" });
    }
    [HttpPost("register")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
            var user = await _authService.RegisterUser(registerDto);
            if (user == null)
            {
                _logger.LogError("Error registering user");
                return BadRequest(new { error = "Error registering user" });
            }
            return Ok(user);
    }

    [HttpPost("login")]
    [ProducesResponseType(200, Type = typeof(LoginResponseDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
            if (User.Identity.IsAuthenticated) return BadRequest(new { error = "You are already signed in with a valid session." });
            
            _logger.LogInformation("Logging in user");
            var response = await _authService.LoginUser(loginDto);
            if (response != null)
            {
                Response.Cookies.Append("X-Access-Token", response.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(7)
                });
                return Ok(new { user = response.User });
            }
            return Unauthorized();
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