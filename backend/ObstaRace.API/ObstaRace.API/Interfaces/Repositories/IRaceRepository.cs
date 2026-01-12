using ObstaRace.API.Dto;
using ObstaRace.API.Models;

namespace ObstaRace.API.Interfaces;

public interface IRaceRepository
{
    //GET
    Task<ICollection<Race>> GetAllRaces(string? difficulty, string? distance, string? search);
    Task<Race?> GetRace(int id);
    Task<Race?> GetRaceBySlug(string slug);
    Task<RaceStatsDto> GetRaceStats();
    Task<bool> RaceExists(int id);
    Task<ICollection<Race>> GetMyRaces(int userId);
    //POST
    Task<bool> CreateRace(Race race);
    //UPDATE
    Task<bool> UpdateRace(Race race,List<RaceObstacle> toAdd, List<RaceObstacle> toRemove);
    //DELETE
    Task<bool> DeleteRace(int raceId);
    //ADDITIONAL METHODS
    Task<bool> SaveChanges();
    Task<bool> RaceNameExists(string name);
    Task<bool> RaceHasRegistrations(int id);
    Task<bool> RaceHasObstacles(int id);
}