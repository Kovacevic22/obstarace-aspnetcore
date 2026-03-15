using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObstaRace.Domain.Models;

public class Participant
{
    [Key, ForeignKey("User")]
    public int UserId { get; set; }

    public User User { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public string? EmergencyContact { get; set; }
    public IList<Registration>? Registrations {get; set;}
}