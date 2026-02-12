using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObstaRace.Domain.Models;

public class Participant
{
    [Key, ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string EmergencyContact { get; set; }
    public IList<Registration> Registrations {get; set;}
}