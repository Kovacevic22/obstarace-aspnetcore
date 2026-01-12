using ObstaRace.API.Dto;
using ObstaRace.API.Models;

namespace ObstaRace.API.Interfaces.Services;

public interface IRaceService
{
    //GET
    Task<ICollection<RaceDto>> GetAllRaces(string? difficulty, string? distanceRange, string? search);
    Task<RaceDto?> GetRace(int id);
    Task<RaceDto?> GetRaceBySlug(string slug);
    Task<RaceStatsDto> GetRaceStats();
    Task<ICollection<RaceDto>> GetMyRaces(int userId);
    //POST
    Task<RaceDto> CreateRace(CreateRaceDto race, int userId);
    //UPDATE
    Task<RaceDto> UpdateRace(UpdateRaceDto race, int id,int userId,Role role);
    //DELETE
    Task<bool> DeleteRace(int raceId, int userId,Role role);
}