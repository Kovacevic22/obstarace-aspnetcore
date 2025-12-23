using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces;
using ObstaRace.API.Models;

namespace ObstaRace.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public UserController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
    public IActionResult GetAllUsers()
    {
        var users = _mapper.Map<List<UserDto>>(_userRepository.GetAllUsers());
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(users);
    }

    [HttpGet("{userId:int}")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(400)]
    public IActionResult GetUserById(int userId)
    {
        if(!_userRepository.UserExists(userId))
            return NotFound();
        var user = _mapper.Map<UserDto>(_userRepository.GetUser(userId));
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(user);
    }
}