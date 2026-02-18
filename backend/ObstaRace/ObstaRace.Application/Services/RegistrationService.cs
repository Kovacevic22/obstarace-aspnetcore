using AutoMapper;
using Microsoft.Extensions.Logging;
using ObstaRace.Application.Dto;
using ObstaRace.Application.Interfaces.Repositories;
using ObstaRace.Application.Interfaces.Services;
using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IRegistrationRepository  _registrationRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRaceRepository _raceRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<RegistrationService> _logger;
    private readonly IEmailService _emailService;
    public RegistrationService(IRegistrationRepository registrationRepository, IMapper mapper, ILogger<RegistrationService> logger, 
        IUserRepository userRepository, IRaceRepository raceRepository, IEmailService emailService)
    {
        _registrationRepository = registrationRepository;
        _mapper = mapper;
        _logger = logger;
        _userRepository = userRepository;
        _raceRepository = raceRepository;
        _emailService = emailService;
    }
    public async Task<ICollection<RegistrationDto>> GetAllRegistrations(int userId, int? page, int? pageSize)
    {
        var registrations = await _registrationRepository.GetAllRegistrations(userId, page, pageSize);
        return _mapper.Map<List<RegistrationDto>>(registrations);
    }
    public async Task<RegistrationDto?> GetRegistration(int id)
    {
        var registration = await _registrationRepository.GetRegistration(id);
        return registration==null?null:_mapper.Map<RegistrationDto>(registration);
    }

    public async Task<ICollection<RegistrationDto>> GetParticipantsForRace(int organiserId, int? raceId, int? page, int? pageSize)
    {
        _logger.LogInformation("Fetching registrations for organiser {Id}, Filter: {RaceId}", organiserId, raceId ?? 0);
    
        var registrations = await _registrationRepository.GetParticipantsForRace(organiserId, raceId, page, pageSize);
    
        return _mapper.Map<List<RegistrationDto>>(registrations);
    }

    public async Task<bool> ConfirmUserRegistration(int registrationId, int organiserId)
    {
        var registration = await _registrationRepository.GetRegistration(registrationId);
        if (registration == null) return false;
        var race = await _raceRepository.GetRace(registration.RaceId);
        if (race == null) return false;
        if (race.CreatedById != organiserId)
        {
            throw new UnauthorizedAccessException("You can only manage participants for your own races");
        }
        registration.Status = RegistrationStatus.Confirmed;
        await  _registrationRepository.UpdateRegistration(registration);
        try
        {
            var user = await _userRepository.GetUser(registration.UserId);
            if (user != null)
            {
                var participantName = user.Participant != null ? $"{user.Participant.Name} {user.Participant.Surname}" : user.Email;
                await _emailService.SendRaceRegistrationApprovedAsync(
                    recipientEmail: user.Email,
                    recipientName: participantName,
                    raceName: race.Name
                );
                
                _logger.LogInformation("Approval email sent to {Email} for race {RaceName}", user.Email, race.Name);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send approval email for registration {RegistrationId}", registrationId);
        }
        return true;
    }

    public async Task<bool> CancelUserRegistration(int registrationId, int organiserId)
    {
        var registration = await _registrationRepository.GetRegistration(registrationId);
        if (registration == null) return false;
        var race = await _raceRepository.GetRace(registration.RaceId);
        if (race == null) return false;
        if (race.CreatedById != organiserId)
        {
            throw new UnauthorizedAccessException("You can only manage participants for your own races");
        }
        registration.Status = RegistrationStatus.Cancelled;
        await  _registrationRepository.UpdateRegistration(registration);
        try
        {
            var user = await _userRepository.GetUser(registration.UserId);
            if (user != null)
            {
                var participantName = user.Participant != null ? $"{user.Participant.Name} {user.Participant.Surname}" : user.Email;

                await _emailService.SendRaceRegistrationRejectedAsync(
                    recipientEmail: user.Email,
                    recipientName: participantName,
                    raceName: race.Name,
                    reason: "Your registration was cancelled by the race organizer. This is most likely due to failed or missing payment, or because the event has reached full capacity."
                );
                
                _logger.LogInformation("Rejection email sent to {Email} for race {RaceName}", user.Email, race.Name);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send rejection email for registration {RegistrationId}", registrationId);
        }
        return true;
    }

    public async Task<int> CountRegistrations(int raceId)
    {
        return await _registrationRepository.CountRegistrations(raceId);
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

        var bibNumber = await _registrationRepository.GetNextBibNumber();
        var registration = new Registration
        {
            UserId = userId,
            RaceId = raceId,
            ParticipantUserId = userId,
            BibNumber = bibNumber.ToString(),
            Status = RegistrationStatus.Pending
        };
        await _registrationRepository.CreateRegistration(registration);
        var fullRegistration = await _registrationRepository.GetRegistration(registration.Id);
        try
        {
            var participantName = user.Participant != null ? $"{user.Participant.Name} {user.Participant.Surname}" : user.Email;
            await _emailService.SendRaceRegistrationConfirmationAsync(
                recipientEmail: user.Email,
                recipientName: participantName,
                raceName: race.Name,
                raceDate: race.Date,
                location: race.Location
                );
            _logger.LogInformation("Confirmation email sent to {Email} for race {RaceName}", user.Email, race.Name);
        }catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send confirmation email for race {raceId}", raceId);
        }
        return _mapper.Map<RegistrationDto>(fullRegistration);
    }
    public async Task<RegistrationDto> UpdateRegistration(UpdateRegistrationDto registration, int id, int userId, Role role)
    {
        _logger.LogInformation("Updating registration {RegistrationId}", id);
        var existingRegistration = await _registrationRepository.GetRegistration(id);
        if (existingRegistration == null)
        {
            _logger.LogWarning("Registration with id {RegistrationId} not found", id);
            throw new ArgumentException("Registration not found");
        }
        var race = await _raceRepository.GetRace(existingRegistration.RaceId);
        if(race==null)throw new ArgumentException("Race not found");
        if ( role != Role.Admin && race.CreatedById != userId)
        {
            throw new UnauthorizedAccessException("You can only manage participants for your own races.");
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
            throw new ArgumentException("Only pending registrations can be canceled.");
        }
        
        return await _registrationRepository.DeleteRegistration(registrationId);
    }

    public async Task<bool> IsUserRegistered(int raceId, int userId)
    {
        _logger.LogInformation("Checking if {RaceId} user {UserId}", raceId, userId);
        return  await _registrationRepository.UserRegistered(userId, raceId);
    }
}