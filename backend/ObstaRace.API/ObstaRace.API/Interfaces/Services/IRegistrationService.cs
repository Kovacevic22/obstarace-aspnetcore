using ObstaRace.API.Dto;
using ObstaRace.API.Models;

namespace ObstaRace.API.Interfaces.Services;

public interface IRegistrationService
{
    Task<ICollection<RegistrationDto>> GetAllRegistrations(int? userId);
    Task<RegistrationDto?> GetRegistration(int id);
    
    //CRUD
    Task<RegistrationDto> CreateRegistration(int raceId, int userId);
    Task<RegistrationDto> UpdateRegistration(UpdateRegistrationDto registration, int id);
    Task<bool> DeleteRegistration(int registrationId);
    
}