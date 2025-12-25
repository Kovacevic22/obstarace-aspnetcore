using ObstaRace.API.Dto;

namespace ObstaRace.API.Interfaces.Services;

public interface IRaceService
{
    //GET
    Task<ICollection<RaceDto>> GetAllRaces();
    Task<RaceDto?> GetRace(int id);
    
    //POST
    Task<RaceDto> CreateRace(RaceDto race);
    //UPDATE
    Task<RaceDto> UpdateRace(RaceDto race, int id);
    //DELETE
    Task<bool> DeleteRace(int raceId);
}