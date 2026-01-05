using ObstaRace.API.Dto;

namespace ObstaRace.API.Interfaces.Services;

public interface IRaceService
{
    //GET
    Task<ICollection<RaceDto>> GetAllRaces(string? difficulty, string? distanceRange, string? search);
    Task<RaceDto?> GetRace(int id);
    Task<RaceDto?> GetRaceBySlug(string slug);
    Task<RaceStatsDto> GetRaceStats();
    //POST
    Task<RaceDto> CreateRace(CreateRaceDto race);
    //UPDATE
    Task<RaceDto> UpdateRace(UpdateRaceDto race, int id);
    //DELETE
    Task<bool> DeleteRace(int raceId);
}