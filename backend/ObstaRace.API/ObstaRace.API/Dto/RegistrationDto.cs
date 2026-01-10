using System.ComponentModel.DataAnnotations;
using ObstaRace.API.Models;

namespace ObstaRace.API.Dto;

public class RegistrationDto
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int RaceId { get; set; }

    [Required]
    public string BibNumber { get; set; }

    [Required]
    public RegistrationStatus Status { get; set; }
    public RaceDto Race { get; set; }
}

public class CreateRegistrationDto
{
    [Required]
    public int RaceId { get; set; }
    [Required]
    public int UserId { get; set; }
}

public class UpdateRegistrationDto
{
    public int RaceId { get; set; }
    public string BibNumber { get; set; }
    public RegistrationStatus Status { get; set; }
    public int Count { get; set; }
    public int UserId { get; set; }
}