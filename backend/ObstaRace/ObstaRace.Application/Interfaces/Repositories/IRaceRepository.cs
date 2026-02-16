
using ObstaRace.Application.Dto;
using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Interfaces.Repositories;

public interface IRaceRepository
{
    //GET
    Task<ICollection<Race>> GetAllRaces(string? difficulty, string? distance, string? search, int? page, int? pageSize);
    Task<Race?> GetRace(int id);
    Task<Race?> GetRaceBySlug(string slug);
    Task<RaceStatsDto> GetRaceStats();
    Task<bool> RaceExists(int id);
    Task<ICollection<Race>> GetMyRaces(int userId, int? page, int? pageSize);
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