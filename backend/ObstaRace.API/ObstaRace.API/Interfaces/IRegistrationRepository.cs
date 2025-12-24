using ObstaRace.API.Models;

namespace ObstaRace.API.Interfaces;

public interface IRegistrationRepository
{
    ICollection<Registration> GetAllRegistrations();
    Registration GetRegistration(int id);
    bool RegistrationExists(int id);
}