using ObstaRace.API.Models;

namespace ObstaRace.API.Interfaces;

public interface IRaceRepository
{
    Task<ICollection<Race>> GetAllRaces();
    Task<Race?> GetRace(int id);
    Task<bool> RaceExists(int id);
}