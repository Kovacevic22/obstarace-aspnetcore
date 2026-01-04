using ObstaRace.API.Models;

namespace ObstaRace.API.Interfaces;

public interface IRaceRepository
{
    //GET
    Task<ICollection<Race>> GetAllRaces(string? difficulty, string? distance, string? search);
    Task<Race?> GetRace(int id);
    Task<bool> RaceExists(int id);
    
    //POST
    Task<bool> CreateRace(Race race);
    //UPDATE
    Task<bool> UpdateRace(Race race);
    //DELETE
    Task<bool> DeleteRace(int raceId);
    //ADDITIONAL METHODS
    Task<bool> SaveChanges();
    Task<bool> RaceNameExists(string name);
    Task<bool> RaceHasRegistrations(int id);
    Task<bool> RaceHasObstacles(int id);
}