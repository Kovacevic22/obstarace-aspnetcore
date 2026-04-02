namespace ObstaRace.Domain.Models;
public enum Role
{
    User = 0,
    Admin = 1,
    Organiser = 2
}
public class User
{
    public int Id { get; init; }
    public string Email { get; init; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string PhoneNumber { get; init; } = null!;
    public string? ProfileImgKey { get; set; }
    public Role Role {get; set; }
    public bool Banned {get; set; }
    public Organiser? Organiser {get; set; }
    public Participant? Participant { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}