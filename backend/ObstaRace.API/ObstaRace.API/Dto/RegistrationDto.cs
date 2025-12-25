using System.ComponentModel.DataAnnotations;
using ObstaRace.API.Models;

namespace ObstaRace.API.Dto;

public class RegistrationDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int RaceId { get; set; }

    [Required]
    public string BibNumber { get; set; }

    [Required]
    public Category Category { get; set; }

    [Required]
    public Status Status { get; set; }
}