using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ObstaRace.Application.Dto;
using ObstaRace.Application.Interfaces.Repositories;
using ObstaRace.Application.Interfaces.Services;
using ObstaRace.Application.Interfaces.Services.Auth;
using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    
    private readonly IValidator<RegisterDto> _validatorRegister;
    private readonly IValidator<LoginDto> _validatorLogin;
    
    public AuthService(ILogger<UserService> logger, IUserRepository userRepository, IMapper mapper, IConfiguration configuration, 
        IEmailService emailService, IValidator<RegisterDto> validatorRegister,  IValidator<LoginDto> validatorLogin)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
        _configuration = configuration;
        _emailService = emailService;
        _validatorRegister = validatorRegister;
        _validatorLogin = validatorLogin;
    }

    public Task<bool> VerifyEmail(string token)
    {
        return _userRepository.VerifyEmail(token);
    }

    public async Task<bool> ResendVerificationEmail(string email)
    {
        var user = await _userRepository.GetUserByEmail(email);
    
        if (user == null || user.Participant == null) return false;
        if (user.Participant.EmailVerified) return false; 
        if (user.Participant.EmailVerificationTokenExpiry > DateTime.UtcNow)
            throw new ArgumentException("Verification email already sent. Please check your inbox or wait for it to expire.");
        var token = Guid.NewGuid().ToString();
        user.Participant.EmailVerificationToken = token;
        user.Participant.EmailVerificationTokenExpiry = DateTime.UtcNow.AddMinutes(5);
    
        await _userRepository.UpdateUser(user);
    
        await _emailService.SendVerificationEmailAsync(
            recipientEmail: user.Email,
            verificationToken: token
        );
    
        return true;
    }

    public async Task<UserDto?> RegisterUser(RegisterDto registerDto)
    {
        var validateResul = await _validatorRegister.ValidateAsync(registerDto);
        if (!validateResul.IsValid)
            throw new ValidationException(validateResul.Errors);
        _logger.LogInformation("Creating user account.");
        var existingUser = await _userRepository.GetUserByEmail(registerDto.Email);
        if (existingUser!=null)
        {
            _logger.LogError("User already exists");
            throw new ArgumentException($"User with email {registerDto.Email} already exists");
        }

        if (!ValidatePassword(registerDto.Password))
        {
            _logger.LogError("Password must contain uppercase, lowercase, number");
            throw new ArgumentException("Password must contain uppercase, lowercase, number");
        }

        var user = _mapper.Map<User>(registerDto);
        user.PasswordHash = PasswordHash(registerDto.Password);
        string? token = null;
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
            token = Guid.NewGuid().ToString();
            user.Participant.EmailVerificationToken = token;
            user.Participant.EmailVerificationTokenExpiry = DateTime.UtcNow.AddMinutes(5);
        }
        _logger.LogInformation("User account created successfully");
        await _userRepository.CreateUser(user);
        if (!string.IsNullOrEmpty(token))
            await _emailService.SendVerificationEmailAsync(
                recipientEmail: user.Email,
                verificationToken: token
            );
        
        return _mapper.Map<UserDto>(user);
    }

    public async Task<LoginResponseDto?> LoginUser(LoginDto loginDto)
    {
        var validateResult = await _validatorLogin.ValidateAsync(loginDto);
        if (!validateResult.IsValid)
            throw new ValidationException(validateResult.Errors);
        _logger.LogInformation("Logging in user with email {Email}", loginDto.Email);
        var user = await _userRepository.GetUserByEmail(loginDto.Email);
        if (user == null)
        {
            _logger.LogError("User does not exist");
            throw new ArgumentException("User does not exist");
        }
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            _logger.LogWarning("Login failed for {Email}", loginDto.Email);
            throw new ArgumentException("Invalid email or password");
        }

        if (user is { Role: Role.User, Participant.EmailVerified: false })
        {
            _logger.LogWarning("User {userId} is not verified", user.Id);
            if (user.Participant.EmailVerificationTokenExpiry < DateTime.UtcNow)
                throw new ArgumentException("Your verification link has expired. Please request a new one.");
            throw new ArgumentException("Please verify your email before logging in.");
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