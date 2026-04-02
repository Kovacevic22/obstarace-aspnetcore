
namespace ObstaRace.Domain.Models;
public enum RegistrationStatus
{
    Pending = 0,    
    Confirmed = 1,  
    Cancelled = 2,  
    Finished = 3    
}
public class Registration
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public int RaceId { get; init; }
    public int ParticipantUserId { get; init; }
    public Participant Participant { get; init; } = null!;
    public User? User { get; init; }
    public Race Race { get; init; } = null!;
    public int BibNumber { get; set; }
    public RegistrationStatus Status { get; set; }
    public bool ReminderSent  { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}