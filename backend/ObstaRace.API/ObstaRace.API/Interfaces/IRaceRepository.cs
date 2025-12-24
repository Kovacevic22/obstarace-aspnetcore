using ObstaRace.API.Models;

namespace ObstaRace.API.Interfaces;

public interface IRaceRepository
{
    ICollection<Race> GetAllRaces();
    Race GetRace(int id);
    bool RaceExists(int id);
}