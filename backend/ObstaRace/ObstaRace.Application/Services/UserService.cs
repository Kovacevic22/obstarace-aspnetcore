using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ObstaRace.Application.Dto;
using ObstaRace.Application.Interfaces.Repositories;
using ObstaRace.Application.Interfaces.Services;
using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IParticipantRepository _participantRepository;
    public UserService(ILogger<UserService> logger, IUserRepository userRepository, IMapper mapper, IConfiguration configuration, IParticipantRepository participantRepository)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
        _configuration = configuration;
        _participantRepository = participantRepository;
    }
    public async Task<ICollection<UserDto>> GetAllUsers(int? page, int? pageSize)
    {
        _logger.LogInformation("Getting all users");
        var users = await _userRepository.GetAllUsers(page, pageSize);
        var userDtos = _mapper.Map<List<UserDto>>(users);
        
        var participantIds = userDtos
            .Where(u => u.Participant != null)
            .Select(u => u.Id)
            .ToList();
        var activities = await _participantRepository.GetActivitiesForUsers(participantIds);
        foreach (var userDto in userDtos.Where(u=>u.Participant != null))
        {
            if (activities.TryGetValue(userDto.Id, out var activity))
            {
                userDto.Participant?.Activity = activity;
            }
        }
        return userDtos;
    }

    public async Task<UserDto?> GetUser(int id)
    {
        _logger.LogInformation("Getting user with id {UserId}", id);
        var user = await _userRepository.GetUser(id);
        if (user == null) return null;
        
        var userDto = _mapper.Map<UserDto>(user);

        if (user.Participant != null)
        {
            userDto.Participant.Activity = await _participantRepository.GetParticipantActivity(id);
        }
        
        return userDto;
    }

    public Task<UserStatsDto> GetUserStats()
    {
        _logger.LogInformation("Getting users stats");
        return  _userRepository.GetUserStats();
    }

    public Task<bool> BanUser(int userId)
    {
        _logger.LogInformation("Banning user with id {UserId}", userId);
        return _userRepository.BanUser(userId);
    }
    public Task<bool> UnbanUser(int userId)
    {
        _logger.LogInformation("Unbanning user with id {UserId}", userId);
        return _userRepository.UnbanUser(userId);
    }
    public async Task<UserDto?> RegisterUser(RegisterDto registerDto)
    {
        _logger.LogInformation("Creating user account.");
        var existingUser = await _userRepository.GetUserByEmail(registerDto.Email);
        if (existingUser!=null)
        {
            _logger.LogError("User with email {Email} already exists", registerDto.Email);
            throw new ArgumentException($"User with email {registerDto.Email} already exists");
        }

        if (!ValidatePassword(registerDto.Password))
        {
            _logger.LogError("Password must contain uppercase, lowercase, number");
            throw new ArgumentException("Password must contain uppercase, lowercase, number");
        }

        var user = _mapper.Map<User>(registerDto);
        user.PasswordHash = PasswordHash(registerDto.Password);
        if (registerDto.Organiser != null)
        {
            user.Role = Role.Organiser;
            user.Organiser.Status = OrganiserStatus.Pending;
            user.Participant = null;
        }
        else
        {
            user.Role = Role.User;
            if (user.Participant == null) throw new ArgumentException("Participant data is required.");
        }
        _logger.LogInformation("User account created successfully");
        await _userRepository.CreateUser(user);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<LoginResponseDto?> LoginUser(LoginDto loginDto)
    {
        _logger.LogInformation("Logging in user with email {Email}", loginDto.Email);
        var user = await _userRepository.GetUserByEmail(loginDto.Email);
        var isPasswordValid = user != null && BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            _logger.LogWarning("Login failed for {Email}", loginDto.Email);
            throw new ArgumentException("Invalid email or password");
        }

        if (user == null)
        {
            _logger.LogError("User does not exist");
            throw new ArgumentException("User does not exist");
        }
        if (user.Banned)
        {
            var displayName = user.Participant != null 
                ? $"{user.Participant.Name} {user.Participant.Surname}" 
                : user.Email;
        
            _logger.LogWarning("Login failed for banned user {Email}", loginDto.Email);
            throw new ArgumentException($"{displayName} is banned!");
        }
        if (user.Role == Role.Organiser && user.Organiser != null)
        {
            if (user.Organiser.Status == OrganiserStatus.Rejected)
            {
                await _userRepository.DeleteUser(user.Id); 
                _logger.LogWarning("Rejected organiser {Email} deleted. They can now re-register.", user.Email);
                throw new ArgumentException("Your previous application was rejected. Your account has been reset, and you can now register again with a better description.");
            }
            if (user.Organiser.Status == OrganiserStatus.Pending)
            {
                throw new ArgumentException("Your account is still pending administrator approval.");
            }
        }
        string token = CreateToken(user);
        return new LoginResponseDto
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddDays(7),
            User = _mapper.Map<UserDto>(user)
        };
    }

    public async Task<UserDto?> UpdateUser(UpdateParticipantDto updateUserDto, int userId)
    {
        _logger.LogInformation("Updating user with id {userId}",userId);
        var user = await _userRepository.GetUser(userId);
        if(user==null)throw new  ArgumentException($"User with id {userId} does not exist");
        if (user.Participant == null) 
            throw new ArgumentException("User does not have a participant profile to update");
        _mapper.Map(updateUserDto, user.Participant);
        if (!await _userRepository.UpdateUser(user))
        {
            _logger.LogInformation("Failed to update user");
            throw new Exception("Failed to update user");
        } 
        return _mapper.Map<UserDto>(user);
    }
    //--------------------------------------------------------//
    private string CreateToken(User user)
    {
        if(user==null) throw new ArgumentNullException(nameof(user));
        var jwtKey = _configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
        {
            _logger.LogError("JWT Key is not configured");
            throw new InvalidOperationException("JWT configuration is missing");
        }
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        var tokenDescriptor = new JwtSecurityToken(
        issuer: _configuration.GetValue<string>("Jwt:Issuer"),
        audience: _configuration.GetValue<string>("Jwt:Audience"),
        claims: claims,
        expires: DateTime.UtcNow.AddDays(7),
        signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
    private string PasswordHash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool ValidatePassword(string password)
    {
        if(password.Length<8)return false;
        bool hasUppercase = password.Any(char.IsUpper);
        bool hasLowercase = password.Any(char.IsLower);
        bool hasNumber = password.Any(char.IsDigit);

        return hasUppercase && hasLowercase && hasNumber;
    }
}