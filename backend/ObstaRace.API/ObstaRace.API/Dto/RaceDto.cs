using System.ComponentModel.DataAnnotations;
using ObstaRace.API.Models;

namespace ObstaRace.API.Dto;

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
}

public class CreateRaceDto
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public double Distance { get; set; }
    public Difficulty Difficulty { get; set; }
    public DateTime RegistrationDeadLine { get; set; }
    public Status Status { get; set; }
    public string ImageUrl { get; set; }
    public int ElevationGain { get; set; }
    public int MaxParticipants { get; set; }
}

public class UpdateRaceDto
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public double Distance { get; set; }
    public Difficulty Difficulty { get; set; }
    public DateTime RegistrationDeadLine { get; set; }
    public Status Status { get; set; }
    public string ImageUrl { get; set; }
    public int ElevationGain { get; set; }
    public int MaxParticipants { get; set; }
}

public class RaceStatsDto
{
    public int TotalRaces { get; set; }
    public double TotalKilometers { get; set; }
    public int ArchivedCount { get; set; }
}