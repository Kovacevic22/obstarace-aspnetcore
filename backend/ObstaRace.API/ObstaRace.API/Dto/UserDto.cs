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