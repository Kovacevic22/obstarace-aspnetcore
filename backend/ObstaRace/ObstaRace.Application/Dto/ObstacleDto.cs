using System.ComponentModel.DataAnnotations;
using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Dto;

public sealed class ObstacleDto
{
    public int Id { get; init; }

    [Required]
    [MaxLength(100)]
    public string Name { get; init; }

    [MaxLength(500)]
    public string Description { get; init; }

    [Required]
    public Difficulty Difficulty { get; init; }
}

public sealed record CreateObstacleDto
{
    [Required]
    [MaxLength(100, ErrorMessage = "Name is too long")]
    public string Name { get; init; }
    [Required]
    [MaxLength(500, ErrorMessage =  "Description is too long")]
    public string Description { get; init; }
    [Required]
    public Difficulty Difficulty { get; init; }
}

public sealed record UpdateObstacleDto
{
    public string Name { get; init; }
    public string Description { get; init; }
    public Difficulty Difficulty { get; init; }
}