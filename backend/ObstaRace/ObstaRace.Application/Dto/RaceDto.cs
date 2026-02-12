using System.ComponentModel.DataAnnotations;
using ObstaRace.Domain.Models;


namespace ObstaRace.Application.Dto;

public class RaceDto
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MaxLength(100)]
    public string Slug { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    [Required]
    [MaxLength(200)]
    public string Location { get; set; }

    [Required]
    [Range(0.1, 1000.0)]
    public double Distance { get; set; }

    [Required]
    public Difficulty Difficulty { get; set; }

    [Required]
    public DateTime RegistrationDeadLine { get; set; }

    [Required]
    public Status Status { get; set; }

    [Url]
    public string ImageUrl { get; set; }

    [Range(0, 10000)]
    public int ElevationGain { get; set; }

    [Required]
    [Range(1, 10000)]
    public int MaxParticipants { get; set; }
    public List<ObstacleDto> Obstacles { get; set; } = new List<ObstacleDto>();
    public List<int> ObstacleIds { get; set; } = new List<int>();
}

public class CreateRaceDto
{
    [Required(ErrorMessage = "Race name is required.")]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [MaxLength(100)]
    public string Slug { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [MaxLength(500, ErrorMessage = "Description is too long.(<500 characters)")]
    public string Description { get; set; }
    [Required]
    [MaxLength(200)]
    public string Location { get; set; }
    [Required]
    [Range(0.1, 1000.0, ErrorMessage = "Distance must be between 0.1 and 1000.")]
    public double Distance { get; set; }
    [Required]
    public Difficulty Difficulty { get; set; }
    [Required]
    public DateTime RegistrationDeadLine { get; set; }
    [Required]
    public Status Status { get; set; }
    [Url(ErrorMessage = "Image url format is not valid.")]
    public string ImageUrl { get; set; }
    [Range(0, 10000, ErrorMessage = "Elevation gain must be between 0 and 10000.")]
    public int ElevationGain { get; set; }
    [Required]
    [Range(1, 10000, ErrorMessage = "Max participants must be between 1 and 1000.")]
    public int MaxParticipants { get; set; }
    public List<int> ObstacleIds { get; set; } = new List<int>();
}

public class UpdateRaceDto
{
    [Required(ErrorMessage = "Race name is required.")]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [MaxLength(100)]
    public string Slug { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [MaxLength(500)]
    public string Description { get; set; }
    [Required]
    [MaxLength(200)]
    public string Location { get; set; }
    [Required]
    [Range(0.1, 1000.0, ErrorMessage = "Distance must be between 0.1 and 1000.")]
    public double Distance { get; set; }
    [Required]
    public Difficulty Difficulty { get; set; }
    [Required]
    public DateTime RegistrationDeadLine { get; set; }
    [Required]
    public Status Status { get; set; }
    [Url(ErrorMessage = "Image url format is not valid.")]
    public string ImageUrl { get; set; }
    [Range(0, 10000, ErrorMessage = "Elevation gain must be between 0 and 10000.")]
    public int ElevationGain { get; set; }
    [Required]
    [Range(1, 10000, ErrorMessage = "Max participants must be between 1 and 1000.")]
    public int MaxParticipants { get; set; }
    public List<int> ObstacleIds { get; set; } = new List<int>();
    
}

public class RaceStatsDto
{
    public int TotalRaces { get; set; }
    public double TotalKilometers { get; set; }
    public int ArchivedCount { get; set; }
}