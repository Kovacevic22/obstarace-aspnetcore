using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Dto;

public sealed record RegistrationDto
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public int RaceId { get; init; }
    public string BibNumber { get; init; } = null!;
    public RegistrationStatus Status { get; init; }
    public RaceDto? Race { get; init; }
    public UserDto? User { get; init; }
    public ParticipantDto? Participant { get; init; }
}

public sealed record CreateRegistrationDto
{
    public int RaceId { get; init; }
    public int UserId { get; init; }
}

public sealed record UpdateRegistrationDto
{
    public int RaceId { get; init; }
    public string? BibNumber { get; init; }
    public RegistrationStatus Status { get; init; }
    public int Count { get; init; }
    public int UserId { get; init; }
}