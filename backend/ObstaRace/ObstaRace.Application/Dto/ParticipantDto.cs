
namespace ObstaRace.Application.Dto;

public  sealed record ParticipantDto
{
    public int UserId { get; init; }
    public string Name { get; init; } = null!;
    public string Surname { get; init; } = null!;
    public DateOnly DateOfBirth { get; init; }
    public string? EmergencyContact { get; init; }
    public ParticipantActivityDto? Activity { get; set; }
}

public sealed record RegisterParticipantDto
{
    public string Name { get; init; } = null!; 
    public string Surname { get; init; } = null!; 
    public DateOnly DateOfBirth { get; init; }
    public string? EmergencyContact { get; init; }
}

public sealed record ParticipantActivityDto
{
    public int TotalRaces { get; init; }
    public int FinishedRaces { get; init; }
}
public sealed record UpdateParticipantDto
{
    public DateOnly DateOfBirth { get; init; }
    public string? EmergencyContact { get; init; }
}