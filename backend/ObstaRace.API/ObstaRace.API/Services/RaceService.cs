using AutoMapper;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces;
using ObstaRace.API.Interfaces.Services;
using ObstaRace.API.Models;
using ObstaRace.API.Repository;

namespace ObstaRace.API.Services;

public class RaceService : IRaceService
{
    private readonly IRaceRepository _raceRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<RaceService> _logger;
    public RaceService(IRaceRepository raceRepository, IMapper mapper, ILogger<RaceService> logger)
    {
        _raceRepository = raceRepository;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<ICollection<RaceDto>> GetAllRaces()
    {
        var races = await _raceRepository.GetAllRaces();
        return _mapper.Map<List<RaceDto>>(races);
    }
    public async Task<RaceDto?> GetRace(int id)
    {
        var race = await _raceRepository.GetRace(id);
        return race==null?null:_mapper.Map<RaceDto>(race);
    }

    public async Task<RaceDto> CreateRace(CreateRaceDto raceDto)
    {
        if (raceDto.Date < DateTime.UtcNow)
        {
            _logger.LogWarning("Cannot create race in the past");
            throw new ArgumentException("Cannot create race in the past");
        }
        if (await _raceRepository.RaceNameExists(raceDto.Name))
        {
            _logger.LogWarning("Race with the same name already exists");
            throw new ArgumentException("Race with the same name already exists");
        }

        var race = _mapper.Map<Race>(raceDto);
        if (!await _raceRepository.CreateRace(race))
        {
            _logger.LogWarning("Failed to create race in database");
            throw new Exception("Failed to create race in database");
        }
        _logger.LogInformation("Race created successfully");
        return _mapper.Map<RaceDto>(race);
    }

    public async Task<RaceDto> UpdateRace(UpdateRaceDto raceDto, int id)
    {
        _logger.LogInformation("Updating race {raceDto}", raceDto.Name);
        var existingRace = await _raceRepository.GetRace(id);
        if (existingRace == null)
        {
            _logger.LogWarning("Race with id {id} does not exist", id);
            throw new ArgumentException($"Race with id {id} does not exist");
        }
        if (raceDto.Date < DateTime.UtcNow)
        {
            _logger.LogWarning("Race date cannot be in the past");
            throw new ArgumentException("Race date cannot be in the past");
        }
        existingRace.Name = raceDto.Name;
        existingRace.Slug = raceDto.Slug;
        existingRace.Date = raceDto.Date;
        existingRace.Description = raceDto.Description;
        existingRace.Location = raceDto.Location;
        existingRace.Distance = raceDto.Distance;
        existingRace.Difficulty = raceDto.Difficulty;
        existingRace.RegistrationDeadLine = raceDto.RegistrationDeadLine;
        existingRace.Status = raceDto.Status;
        existingRace.ImageUrl = raceDto.ImageUrl;
        existingRace.ElevationGain = raceDto.ElevationGain;
        existingRace.MaxParticipants = raceDto.MaxParticipants;
        if (!await _raceRepository.UpdateRace(existingRace))
        {
            _logger.LogError("Failed to update race in database");
            throw new Exception("Failed to update race in database");
        }
        return _mapper.Map<RaceDto>(existingRace);
    }

    public async Task<bool> DeleteRace(int raceId)
    {
        _logger.LogInformation("Deleting race {raceId}", raceId);
        if (!await _raceRepository.RaceExists(raceId))
        {
            _logger.LogWarning("Race with id {raceId} does not exist", raceId);
            throw new ArgumentException($"Race with id {raceId} does not exist");
        }

        if (await _raceRepository.RaceHasObstacles(raceId))
        {
            _logger.LogWarning("Race has obstacles");
            throw new ArgumentException("Race has obstacles");
        }

        if (await _raceRepository.RaceHasRegistrations(raceId))
        {
            _logger.LogWarning("Race has registrations");
            throw new ArgumentException("Race has registrations");
        }
        return await _raceRepository.DeleteRace(raceId);
    }
}