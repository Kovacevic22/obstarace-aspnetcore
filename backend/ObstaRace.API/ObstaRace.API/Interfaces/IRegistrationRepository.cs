using ObstaRace.API.Models;

namespace ObstaRace.API.Interfaces;

public interface IRegistrationRepository
{
    Task<ICollection<Registration>> GetAllRegistrations();
    Task<Registration?> GetRegistration(int id);
    Task<bool> RegistrationExists(int id);
}