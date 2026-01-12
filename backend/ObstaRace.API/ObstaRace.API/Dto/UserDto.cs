using System.ComponentModel.DataAnnotations;
using ObstaRace.API.Models;

namespace ObstaRace.API.Dto;



public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber {get; set; }
    public DateTime DateOfBirth { get; set; }
    public string EmergencyContact { get; set; }
    public Role Role { get; set; }
    public bool Banned { get; set; }
    public OrganiserDto? Organiser { get; set; }
    public UserActivityDto Activity { get; set; }
}

public class RegisterDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [Phone]
    public string PhoneNumber {get; set; }
    [Required]
    [MinLength(8)]
    public string Password { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
    [Required]
    [Phone]
    public string EmergencyContact { get; set; }
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

public class UpdateUserDto
{
    [Required]
    [Phone]
    public string PhoneNumber {get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
    [Required]
    [Phone]
    public string EmergencyContact { get; set; }
}
public class UserActivityDto
{
    public int TotalRaces { get; set; }
    public int FinishedRaces { get; set; }
}