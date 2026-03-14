using System.ComponentModel.DataAnnotations;
using ObstaRace.Domain.Models;


namespace ObstaRace.Application.Dto;



public sealed record UserDto
{
    public int Id { get; init; }
    public string Email { get; init; }
    public string PhoneNumber {get; init; }
    public Role Role { get; init; }
    public bool Banned { get; init; }
    public OrganiserDto? Organiser { get; init; }
    public ParticipantDto?  Participant { get; init; }
}

public sealed record RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; init; }
    [Required]
    [Phone]
    public string PhoneNumber {get; init; }
    [Required]
    [MinLength(8)]
    public string Password { get; init; }
    public RegisterParticipantDto? Participant { get; init; }
    public RegisterOrganiserDto? Organiser { get; init; }
}

public sealed record LoginDto
{
    [Required]
    public string Email { get; init; }
    [Required]
    public string Password { get; init; }
}
public sealed record LoginResponseDto
{
    public string Token { get; init; }
    public DateTime Expiration { get; init; }
    public UserDto User { get; init; }
}

public sealed record UserStatsDto
{
    public int TotalUsers { get; init; }
    public int BannedUsers { get; init; }
}
