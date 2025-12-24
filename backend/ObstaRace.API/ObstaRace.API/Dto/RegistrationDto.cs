using ObstaRace.API.Models;

namespace ObstaRace.API.Dto;

public class RegistrationDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RaceId { get; set; }
    public string BibNumber { get; set; }
    public Category  Category { get; set; }
    public Status Status { get; set; }
}