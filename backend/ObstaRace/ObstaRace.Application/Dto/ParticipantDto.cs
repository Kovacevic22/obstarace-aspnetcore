using System.ComponentModel.DataAnnotations;

namespace ObstaRace.Application.Dto;

public  sealed record ParticipantDto
{
    public int UserId { get; init; }
    public string Name { get; init; }
    public string Surname { get; init; }
    public DateTime DateOfBirth { get; init; }
    public string EmergencyContact { get; init; }
    public ParticipantActivityDto? Activity { get; init; }
}

public sealed record RegisterParticipantDto
{
    [Required]
    public string Name { get; init; }
    [Required]
    public string Surname { get; init; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; init; }
    [Required]
    [Phone]
    public string EmergencyContact { get; init; }
}

public sealed record ParticipantActivityDto
{
    public int TotalRaces { get; init; }
    public int FinishedRaces { get; init; }
}
public sealed record UpdateParticipantDto
{
    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; init; }
    [Required]
    [Phone]
    public string EmergencyContact { get; init; }
}