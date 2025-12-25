using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces;
using ObstaRace.API.Models;

namespace ObstaRace.API.Controllers;
[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserController> _logger;
    public UserController(IUserRepository userRepository, IMapper mapper, ILogger<UserController> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
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
            var users = _mapper.Map<List<UserDto>>(await _userRepository.GetAllUsers());
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return StatusCode(500, "Error retrieving users");
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
            var user = _mapper.Map<UserDto>(await _userRepository.GetUser(userId));
            if (user == null)
            {
                _logger.LogWarning("User with id {UserId} not found", userId);
                return NotFound($"User with id {userId} not found");
            }

            return Ok(user);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with id {UserId}", userId);
            return StatusCode(500, "Error retrieving user");
        }
    }
}