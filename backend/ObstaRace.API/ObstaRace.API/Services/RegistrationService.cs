using AutoMapper;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces;
using ObstaRace.API.Interfaces.Services;
using ObstaRace.API.Models;
using ObstaRace.API.Repository;

namespace ObstaRace.API.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IRegistrationRepository  _registrationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRaceRepository _raceRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<RegistrationService> _logger;
    public RegistrationService(IRegistrationRepository registrationRepository, IMapper mapper, ILogger<RegistrationService> logger, IUserRepository userRepository, IRaceRepository raceRepository)
    {
        _registrationRepository = registrationRepository;
        _mapper = mapper;
        _logger = logger;
        _userRepository = userRepository;
        _raceRepository = raceRepository;
    }
    public async Task<ICollection<RegistrationDto>> GetAllRegistrations(int? userId)
    {
        var registrations = await _registrationRepository.GetAllRegistrations(userId);
        return _mapper.Map<List<RegistrationDto>>(registrations);
    }
    public async Task<RegistrationDto?> GetRegistration(int id)
    {
        var registration = await _registrationRepository.GetRegistration(id);
        return registration==null?null:_mapper.Map<RegistrationDto>(registration);
    }

    public async Task<RegistrationDto> CreateRegistration(int raceId, int userId)
    {
        if (raceId <= 0 || userId <= 0)
            throw new ArgumentException("Invalid raceId or userId");
        var user = await _userRepository.GetUser(userId);
        if (user == null)
        {
            _logger.LogWarning("User with id {UserId} does not exist", userId);
            throw new ArgumentException("User does not exist");
        }
        var race = await _raceRepository.GetRace(raceId);
        if (race==null)
        {
            _logger.LogWarning("Race with id {RaceId} does not exist", raceId);
            throw new ArgumentException("Race does not exist");
        }

        if (await _registrationRepository.UserRegistered(userId, raceId))
        {
            _logger.LogWarning("User with id {UserId} has already registered for race {RaceId}", userId, raceId);
            throw new ArgumentException("User has already registered for race");
        }
        if (race.RegistrationDeadLine.Date < DateTime.UtcNow.Date)
        {
            _logger.LogWarning("Registration deadline for race {RaceId} has passed", raceId);
            throw new ArgumentException("Registration deadline has passed");
        }

        var count = await _registrationRepository.CountRegistrations(raceId);
        if (count >= race.MaxParticipants)
        {
            _logger.LogWarning("Race {RaceId} has reached maximum participants", raceId);
            throw new ArgumentException("Race has reached maximum participants");
        }
        var bibNumber = GenerateBibNumber(raceId, count);
        var registration = new Registration
        {
            UserId = userId,
            RaceId = raceId,
            BibNumber = bibNumber.ToString(),
            Status = RegistrationStatus.Pending
        };
        await _registrationRepository.CreateRegistration(registration);
        return _mapper.Map<RegistrationDto>(registration);
    }
    public async Task<RegistrationDto> UpdateRegistration(UpdateRegistrationDto registration, int id)
    {
        _logger.LogInformation("Updating registration {RegistrationId}", id);
        var existingRegistration = await _registrationRepository.GetRegistration(id);
        if (existingRegistration == null)
        {
            _logger.LogWarning("Registration with id {RegistrationId} not found", id);
            throw new ArgumentException("Registration not found");
        }
        existingRegistration.Status = registration.Status;
        await _registrationRepository.UpdateRegistration(existingRegistration);
        return _mapper.Map<RegistrationDto>(existingRegistration);
    }

    public async Task<bool> DeleteRegistration(int registrationId, int currentUserId)
    {
        _logger.LogInformation("Deleting registration {RegistrationId}", registrationId);
        var registration = await _registrationRepository.GetRegistration(registrationId);
        if (registration == null)
        {
            _logger.LogWarning("Registration with id {RegistrationId} not found", registrationId);
            throw new ArgumentException("Registration not found");
        }
        if (registration.UserId != currentUserId)
        {
            throw new UnauthorizedAccessException("You can only delete your own registrations.");
        }
        if (registration.Status != RegistrationStatus.Pending) 
        {
            _logger.LogWarning("Registration {RegistrationId} cannot be deleted because it is already confirmed/finished", registrationId);
            throw new ArgumentException("Only pending registrations can be deleted.");
        }
        
        return await _registrationRepository.DeleteRegistration(registrationId);
    }
    //ADDITIONAL METHODS
    private int GenerateBibNumber(int raceId, int count)
    {
        return (raceId*100) + (count + 1);
    }
}