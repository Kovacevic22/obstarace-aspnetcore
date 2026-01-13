using System.ComponentModel.DataAnnotations;
using ObstaRace.API.Models;

namespace ObstaRace.API.Dto;



public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string PhoneNumber {get; set; }
    public Role Role { get; set; }
    public bool Banned { get; set; }
    public OrganiserDto? Organiser { get; set; }
    public ParticipantDto?  Participant { get; set; }
}

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [Phone]
    public string PhoneNumber {get; set; }
    [Required]
    [MinLength(8)]
    public string Password { get; set; }
    public RegisterParticipantDto? Participant { get; set; }
    public RegisterOrganiserDto? Organiser { get; set; }
}

public class LoginDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}
public class LoginResponseDto
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public UserDto User { get; set; }
}

public class UserStatsDto
{
    public int TotalUsers { get; set; }
    public int BannedUsers { get; set; }
}
