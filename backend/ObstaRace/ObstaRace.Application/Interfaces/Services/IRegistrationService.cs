

using ObstaRace.Application.Dto;
using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Interfaces.Services;

public interface IRegistrationService
{
    Task<ICollection<RegistrationDto>> GetAllRegistrations(int userId);
    Task<RegistrationDto?> GetRegistration(int id);
    Task<ICollection<RegistrationDto>> GetParticipantsForRace(int organiserId, int? raceId);
    Task<bool> ConfirmUserRegistration(int registrationId, int organiserId);
    Task<bool> CancelUserRegistration(int registrationId, int organiserId);

    Task<int> CountRegistrations(int raceId);
    //CRUD
    Task<RegistrationDto> CreateRegistration(int raceId, int userId);
    Task<RegistrationDto> UpdateRegistration(UpdateRegistrationDto registration, int id,int userId, Role role);
    Task<bool> DeleteRegistration(int registrationId,int currentUserId);
    
}