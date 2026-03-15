using System.ComponentModel.DataAnnotations;
using ObstaRace.Domain.Models;


namespace ObstaRace.Application.Dto;



public sealed record UserDto
{
    public int Id { get; init; }
    public string Email { get; init; } = null!;
    public string PhoneNumber { get; init; } = null!;
    public Role Role { get; init; }
    public bool Banned { get; init; }
    public OrganiserDto? Organiser { get; init; }
    public ParticipantDto?  Participant { get; init; }
}

public sealed record RegisterDto
{
    [Required] [EmailAddress] public string Email { get; init; } = null!;
    [Required] [Phone] public string PhoneNumber { get; init; } = null!;
    [Required] [MinLength(8)] public string Password { get; init; } = null!;
    public RegisterParticipantDto? Participant { get; init; }
    public RegisterOrganiserDto? Organiser { get; init; }
}

public sealed record LoginDto
{
    [Required] public string Email { get; init; } = null!;
    [Required]
    public string Password { get; init; } = null!;
}
public sealed record LoginResponseDto
{
    public string Token { get; init; } = null!;
    public DateTime Expiration { get; init; }
    public UserDto? User { get; init; }
}

public sealed record UserStatsDto
{
    public int TotalUsers { get; init; }
    public int BannedUsers { get; init; }
}
