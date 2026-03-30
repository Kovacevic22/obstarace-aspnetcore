using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ObstaRace.Application.Dto;
using ObstaRace.Application.Interfaces.Repositories;
using ObstaRace.Application.Interfaces.Services;

namespace ObstaRace.Application.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IParticipantRepository _participantRepository;
    private readonly IDistributedCache _cache;
    
    private readonly IValidator<UpdateParticipantDto> _validatorUpdate;
    
    public UserService(ILogger<UserService> logger, IUserRepository userRepository, IMapper mapper,
        IParticipantRepository participantRepository, IDistributedCache cache, IValidator<UpdateParticipantDto> validatorUpdate)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
        _participantRepository = participantRepository;
        _cache = cache;
        _validatorUpdate = validatorUpdate;
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
                if (userDto.Participant != null) userDto.Participant.Activity = activity;
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

    public async Task<bool> BanUser(int userId)
    {   
        _logger.LogInformation("Banning user with id {UserId}", userId);
        var result = await _userRepository.BanUser(userId);
        if (result) await _cache.RemoveAsync($"ban_{userId}");
        return result;
    }
    public async Task<bool> UnbanUser(int userId)
    {
        _logger.LogInformation("Unbanning user with id {UserId}", userId);
        var result = await _userRepository.UnbanUser(userId);
        if (result) await _cache.RemoveAsync($"ban_{userId}"); 
        return result;
    }

    public async Task<UserDto?> UpdateProfileImage(int userId, string key)
    {
        _logger.LogInformation("Updating profile image for user {UserId}", userId);
        var user = await _userRepository.GetUser(userId);
        if (user == null) throw new ArgumentException($"User with id {userId} does not exist");
        user.ProfileImgKey = key;
        if (!await _userRepository.UpdateUser(user))
            throw new Exception("Failed to update profile image");
        return _mapper.Map<UserDto>(user);    
    }
    public async Task<UserDto?> UpdateUser(UpdateParticipantDto updateUserDto, int userId)
    {
        var validatorResult = await  _validatorUpdate.ValidateAsync(updateUserDto);
        if (!validatorResult.IsValid)
            throw new ValidationException(validatorResult.Errors);
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
}