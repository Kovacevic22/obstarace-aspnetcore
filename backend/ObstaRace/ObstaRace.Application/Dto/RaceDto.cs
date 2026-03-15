using System.ComponentModel.DataAnnotations;
using ObstaRace.Domain.Models;


namespace ObstaRace.Application.Dto;

public sealed record RaceDto
{
    public int Id { get; init; }

    [Required] [MaxLength(100)] public string Name { get; init; } = null!;

    [Required] [MaxLength(100)] public string Slug { get; init; } = null!;

    [Required]
    public DateTime Date { get; init; }

    [MaxLength(500)]
    public string? Description { get; init; }

    [Required] [MaxLength(200)] public string Location { get; init; } = null!;

    [Required]
    [Range(0.1, 1000.0)]
    public double Distance { get; init; }

    [Required]
    public Difficulty Difficulty { get; init; }

    [Required]
    public DateTime RegistrationDeadLine { get; init; }

    [Required]
    public Status Status { get; init; }

    [Url]
    public string? ImageUrl { get; init; }

    [Range(0, 10000)]
    public int ElevationGain { get; init; }

    [Required]
    [Range(1, 10000)]
    public int MaxParticipants { get; init; }
    public List<ObstacleDto> Obstacles { get; init; } = new List<ObstacleDto>();
    public List<int> ObstacleIds { get; init; } = new List<int>();
}

public sealed record CreateRaceDto
{
    [Required(ErrorMessage = "Race name is required.")]
    [MaxLength(100)]
    public string Name { get; init; } = null!;

    [Required] [MaxLength(100)] public string Slug { get; init; } = null!;
    [Required]
    public DateTime Date { get; init; }
    [MaxLength(500, ErrorMessage = "Description is too long.(<500 characters)")]
    public string? Description { get; init; }

    [Required] [MaxLength(200)] public string Location { get; init; } = null!;
    [Required]
    [Range(0.1, 1000.0, ErrorMessage = "Distance must be between 0.1 and 1000.")]
    public double Distance { get; init; }
    [Required]
    public Difficulty Difficulty { get; init; }
    [Required]
    public DateTime RegistrationDeadLine { get; init; }
    [Required]
    public Status Status { get; init; }
    [Url(ErrorMessage = "Image url format is not valid.")]
    public string? ImageUrl { get; init; }
    [Range(0, 10000, ErrorMessage = "Elevation gain must be between 0 and 10000.")]
    public int ElevationGain { get; init; }
    [Required]
    [Range(1, 10000, ErrorMessage = "Max participants must be between 1 and 1000.")]
    public int MaxParticipants { get; init; }
    public List<int> ObstacleIds { get; init; }
}

public sealed record UpdateRaceDto
{
    [Required(ErrorMessage = "Race name is required.")]
    [MaxLength(100)]
    public string Name { get; init; } = null!;

    [Required] [MaxLength(100)] public string Slug { get; init; } = null!;
    [Required]
    public DateTime Date { get; init; }
    [MaxLength(500)]
    public string? Description { get; init; }

    [Required] [MaxLength(200)] public string Location { get; init; } = null!;
    [Required]
    [Range(0.1, 1000.0, ErrorMessage = "Distance must be between 0.1 and 1000.")]
    public double Distance { get; init; }
    [Required]
    public Difficulty Difficulty { get; init; }
    [Required]
    public DateTime RegistrationDeadLine { get; init; }
    [Required]
    public Status Status { get; init; }
    [Url(ErrorMessage = "Image url format is not valid.")]
    public string? ImageUrl { get; init; }
    [Range(0, 10000, ErrorMessage = "Elevation gain must be between 0 and 10000.")]
    public int ElevationGain { get; init; }
    [Required]
    [Range(1, 10000, ErrorMessage = "Max participants must be between 1 and 1000.")]
    public int MaxParticipants { get; init; }
    public List<int> ObstacleIds { get; init; }
    
}

public sealed record RaceStatsDto
{
    public int TotalRaces { get; init; }
    public double TotalKilometers { get; init; }
    public int ArchivedCount { get; init; }
}