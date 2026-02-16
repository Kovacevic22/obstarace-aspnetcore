
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
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RaceId { get; set; }
    public int ParticipantUserId { get; set; }
    public Participant Participant { get; set; }
    public User User { get; set; }
    public Race Race { get; set; }
    public string BibNumber { get; set; }
    public RegistrationStatus Status { get; set; }
    public bool ReminderSent  { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}