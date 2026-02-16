namespace ObstaRace.Domain.Models;
public enum Role
{
    User = 0,
    Admin = 1,
    Organiser = 2
}
public class User
{
    public int Id {get; set; }
    public string Email {get; set; }
    public string PasswordHash {get; set; }
    public string PhoneNumber {get; set; }
    public Role Role {get; set; }
    public bool Banned {get; set; }
    public Organiser? Organiser {get; set; }
    public Participant? Participant { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}