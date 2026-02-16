
using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Interfaces.Repositories;

public interface IRegistrationRepository
{
    Task<ICollection<Registration>> GetAllRegistrations(int? userId, int? page, int? pageSize);
    Task<Registration?> GetRegistration(int id);
    Task<bool> RegistrationExists(int id);
    Task<ICollection<Registration>> GetParticipantsForRace(int organiserId, int? raceId, int? page, int? pageSize);
    //CRUD
    Task<bool> CreateRegistration(Registration registration);
    Task<bool> UpdateRegistration(Registration registration);
    Task<bool> DeleteRegistration(int registrationId);
    Task<List<Registration>> GetRegistrationsForReminderAsync(DateTime targetDate);
    //ADDITIONAL METHODS
    Task<bool> SaveChanges();
    Task<Registration?> GetRegistrationByUserId(int userId);
    Task<List<Registration>?> GetRegistrationsByRaceId(int raceId);
    Task<bool> UserRegistered(int userId, int raceId);
    Task<int> CountRegistrations(int raceId);
}