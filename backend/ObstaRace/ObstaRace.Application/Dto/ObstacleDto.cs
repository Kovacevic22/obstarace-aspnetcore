using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Dto;

public sealed record ObstacleDto
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public Difficulty Difficulty { get; init; }
}

public sealed record CreateObstacleDto
{
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public Difficulty Difficulty { get; init; }
}

public sealed record UpdateObstacleDto
{
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public Difficulty Difficulty { get; init; }
}