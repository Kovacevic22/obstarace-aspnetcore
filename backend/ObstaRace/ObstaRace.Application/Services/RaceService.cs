using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ObstaRace.Application.Dto;
using ObstaRace.Application.Interfaces.Repositories;
using ObstaRace.Application.Interfaces.Services;
using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Services;

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
    public async Task<ICollection<RaceDto>> GetAllRaces(string? difficulty, string? distanceRange, string? search, int? page, int? pageSize)
    {
        var races = await _raceRepository.GetAllRaces(difficulty,distanceRange,search, page, pageSize);
        return _mapper.Map<List<RaceDto>>(races);
    }
    public async Task<RaceDto?> GetRace(int id)
    {
        var race = await _raceRepository.GetRace(id);
        if (race == null) return null;
        var raceDto = _mapper.Map<RaceDto>(race);
        raceDto.ObstacleIds = race.RaceObstacles
            .Select(ro => ro.ObstacleId)
            .ToList();

        return raceDto;
    }

    public async Task<RaceDto?> GetRaceBySlug(string slug)
    {
        var race = await _raceRepository.GetRaceBySlug(slug);
        return race == null ? null : _mapper.Map<RaceDto>(race);
    }

    public async Task<ICollection<RaceDto>> GetMyRaces(int userId, int? page, int? pageSize)
    {
        _logger.LogInformation("Retrieving races created by {id}",userId);
        var races = await _raceRepository.GetMyRaces(userId, page,pageSize);
        return _mapper.Map<List<RaceDto>>(races);
    }

    public async Task<RaceDto> CreateRace(CreateRaceDto raceDto, int userId)
    {
        var slug = raceDto.Slug.ToLower().Trim();
        slug = Regex.Replace(slug,@"[^a-z0-9]+", "-");
        slug = slug.Trim('-');
        raceDto.Slug = slug;
        if (await _raceRepository.GetRaceBySlug(raceDto.Slug) != null)
        {
            _logger.LogError("Cannot create race with that slug");
            throw new ArgumentException("Race with that slug already exist");
        }
        if (raceDto.Date.Date < DateTime.UtcNow.Date)
        {
            _logger.LogWarning("Cannot create race in the past");
            throw new ArgumentException("Cannot create race in the past");
        }

        if (raceDto.Date.Date < raceDto.RegistrationDeadLine.Date)
        {
            _logger.LogWarning("Cannot create race! Deadline must be rather then date!");
            throw new ArgumentException("Cannot create race! Deadline must be rather then date!");
        }

        if (await _raceRepository.RaceNameExists(raceDto.Name))
        {
            _logger.LogWarning("Race with the same name already exists");
            throw new ArgumentException("Race with the same name already exists");
        }

        var race = _mapper.Map<Race>(raceDto);
        race.CreatedById = userId;
        if (raceDto.ObstacleIds.Any())
        {
            race.RaceObstacles = raceDto.ObstacleIds.Select(id => new RaceObstacle()
            {
                Race = race,
                ObstacleId = id,
            }).ToList();
        }
        if (!await _raceRepository.CreateRace(race))
        {
            _logger.LogWarning("Failed to create race in database");
            throw new Exception("Failed to create race in database");
        }
        _logger.LogInformation("Race created successfully");
        return _mapper.Map<RaceDto>(race);
    }

    public async Task<RaceStatsDto> GetRaceStats()
    {
        _logger.LogInformation("Calculating race statistics from repository");
        return await _raceRepository.GetRaceStats();
    }
    public async Task<RaceDto> UpdateRace(UpdateRaceDto raceDto, int id,int userId, Role role)
    {
        _logger.LogInformation("Updating race {raceDto}", raceDto.Name);
        var existingRace = await _raceRepository.GetRace(id);
        if (existingRace == null)
        {
            _logger.LogWarning("Race with id {id} does not exist", id);
            throw new ArgumentException($"Race with id {id} does not exist");
        }
        if (role != Role.Admin && existingRace.CreatedById != userId)
        {
            throw new UnauthorizedAccessException("You can only edit your own races.");
        }
        if (raceDto.Date.Date < DateTime.UtcNow.Date)
        {
            _logger.LogWarning("Race date cannot be in the past");
            throw new ArgumentException("Race date cannot be in the past");
        }
        if (raceDto.RegistrationDeadLine.Date > raceDto.Date.Date)
        {
            _logger.LogWarning("Registration deadline cannot be after the race date");
            throw new ArgumentException("Registration deadline cannot be after the race date");
        }
        var raceWithSameSlug = await _raceRepository.GetRaceBySlug(raceDto.Slug);
        if (raceWithSameSlug != null && raceWithSameSlug.Id != id)
        {
            throw new ArgumentException("Race with this URL identifier already exists!");
        }
        var currentObstacleIds = existingRace.RaceObstacles
            .Select(ro => ro.ObstacleId)
            .ToList();
        var toRemove = existingRace.RaceObstacles
            .Where(ro => !raceDto.ObstacleIds.Contains(ro.ObstacleId))
            .ToList();
        var toAdd = raceDto.ObstacleIds
            .Where(idC => !currentObstacleIds.Contains(idC))
            .Select(idN => new RaceObstacle { RaceId = existingRace.Id, ObstacleId = idN })
            .ToList();
        _mapper.Map(raceDto, existingRace);
        if (!await _raceRepository.UpdateRace(existingRace,toAdd, toRemove))
        {
            _logger.LogError("Failed to update race in database");
            throw new Exception("Failed to update race in database");
        }
        return _mapper.Map<RaceDto>(existingRace);
    }

    public async Task<bool> DeleteRace(int raceId, int userId,Role role)
    {
        _logger.LogInformation("Deleting race {raceId}", raceId);
        var race = await _raceRepository.GetRace(raceId);
        if (race == null) return false;
        if (race.CreatedById != userId && role != Role.Admin)
        {
            throw new UnauthorizedAccessException("You can only delete your own races");
        }
        return await _raceRepository.DeleteRace(raceId);
    }
}