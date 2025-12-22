namespace ObstaRace.API.Models;
public enum Role
{
    User,
    Admin,
    Organizer
}
public class User
{
    public int Id {get; set; }
    public string Name {get; set; }
    public string Surname {get; set; }
    public string Email {get; set; }
    public string Password {get; set; }
    public string PhoneNumber {get; set; }
    public DateTime DateOfBirth { get; set; }
    public string EmergencyContact { get; set; }
    public Role Role {get; set; }
    public IList<Registration> Registrations {get; set;}
}