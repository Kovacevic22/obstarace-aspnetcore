using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces;
using ObstaRace.API.Interfaces.Services;
using ObstaRace.API.Models;

namespace ObstaRace.API.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    public UserService(ILogger<UserService> logger, IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
        _configuration = configuration;
    }
    public async Task<ICollection<UserDto>> GetAllUsers()
    {
        _logger.LogInformation("Getting all users");
        var users = await _userRepository.GetAllUsers();
        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<UserDto?> GetUser(int id)
    {
        _logger.LogInformation("Getting user with id {UserId}", id);
        var user = await _userRepository.GetUser(id);
        return user==null?null:_mapper.Map<UserDto>(user);
    }

    public Task<UserStatsDto> GetUserStats()
    {
        _logger.LogInformation("Getting users stats");
        return  _userRepository.GetUserStats();
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
        string hashPassword = PasswordHash(registerDto.Password);
        var user = new User()
        {
            Name = registerDto.Name,
            Surname = registerDto.Surname,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            PasswordHash = hashPassword,
            DateOfBirth = registerDto.DateOfBirth,
            EmergencyContact = registerDto.EmergencyContact,
            Role = Role.User
        };
        _logger.LogInformation("User account created successfully");
        await _userRepository.CreateUser(user);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<LoginResponseDto?> LoginUser(LoginDto loginDto)
    {
        _logger.LogInformation("Logging in user with email {Email}", loginDto.Email);
        var user = await _userRepository.GetUserByEmail(loginDto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            _logger.LogWarning("Login failed for {Email}", loginDto.Email);
            throw new ArgumentException($"Login failed for {loginDto.Email}");
        }
        string token = CreateToken(user);
        return new LoginResponseDto
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddDays(7),
            User = _mapper.Map<UserDto>(user)
        };
    }

    public async Task<UserDto?> UpdateUser(UpdateUserDto updateUserDto, int userId)
    {
        _logger.LogInformation("Updating user with id {userId}",userId);
        var user = await _userRepository.GetUser(userId);
        if(user==null)throw new  ArgumentException($"User with id {userId} does not exist");
        _mapper.Map(updateUserDto, user);
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