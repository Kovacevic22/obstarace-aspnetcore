using System.ComponentModel.DataAnnotations;
using ObstaRace.API.Models;

namespace ObstaRace.API.Dto;

public class RaceDto
{
    [Required]
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