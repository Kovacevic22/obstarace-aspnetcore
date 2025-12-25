using ObstaRace.API.Models;

namespace ObstaRace.API.Interfaces;

public interface IRegistrationRepository
{
    Task<ICollection<Registration>> GetAllRegistrations();
    Task<Registration?> GetRegistration(int id);
    Task<bool> RegistrationExists(int id);
    
    //CRUD
    Task<bool> CreateRegistration(Registration registration);
    Task<bool> UpdateRegistration(Registration registration);
    Task<bool> DeleteRegistration(int registrationId);
    
    //ADDITIONAL METHODS
    Task<bool> SaveChanges();
    Task<Registration?> GetRegistrationByUserId(int userId);
    Task<List<Registration>?> GetRegistrationsByRaceId(int raceId);
    Task<bool> UserRegistered(int userId, int raceId);
    Task<int> CountRegistrations(int raceId);
}