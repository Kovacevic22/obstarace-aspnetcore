using ObstaRace.API.Dto;
using ObstaRace.API.Models;

namespace ObstaRace.API.Interfaces.Services;

public interface IRegistrationService
{
    Task<ICollection<RegistrationDto>> GetAllRegistrations();
    Task<RegistrationDto?> GetRegistration(int id);
    
    //CRUD
    Task<RegistrationDto> CreateRegistration(int raceId, int userId, Category category);
    Task<RegistrationDto> UpdateRegistration(UpdateRegistrationDto registration, int id);
    Task<bool> DeleteRegistration(int registrationId);
    
}