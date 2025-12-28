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
}

public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}
public class LoginResponseDto
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public UserDto User { get; set; }
}