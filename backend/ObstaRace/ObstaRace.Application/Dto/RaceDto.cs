using ObstaRace.Domain.Models;


namespace ObstaRace.Application.Dto;

public sealed record RaceDto
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public DateTime Date { get; init; }
    public string? Description { get; init; } 
    public string Location { get; init; } = null!;
    public double Distance { get; init; }
    public Difficulty Difficulty { get; init; }
    public DateTime RegistrationDeadLine { get; init; }
    public Status Status { get; init; } 
    public string? ImageUrl { get; init; }
    public int ElevationGain { get; init; }
    public int MaxParticipants { get; init; }
    public List<ObstacleDto> Obstacles { get; init; } = new List<ObstacleDto>();
    public List<int> ObstacleIds { get; init; } = new List<int>();
}

public record CreateRaceDto
{ 
    public string Name { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public string Location { get; init; } = null!;
    public DateTime Date { get; init; }
    public string? Description { get; init; }
    public double Distance { get; init; }
    public Difficulty Difficulty { get; init; }
    public DateTime RegistrationDeadLine { get; init; }
    public Status Status { get; init; }
    public string? ImageUrl { get; init; }
    public int ElevationGain { get; init; }
    public int MaxParticipants { get; init; }
    public List<int> ObstacleIds { get; init; } = null!;
}

public sealed record UpdateRaceDto : CreateRaceDto { }
public sealed record RaceStatsDto
{
    public int TotalRaces { get; init; }
    public double TotalKilometers { get; init; }
    public int ArchivedCount { get; init; }
}