using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObstaRace.Domain.Models;

public class Participant
{
    [Key, ForeignKey("User")]
    public int UserId { get; init; }

    public User User { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Surname { get; init; } = null!;
    public DateOnly DateOfBirth { get; init; }
    public string? EmergencyContact { get; init; }
    public bool EmailVerified { get; set; }
    public string? EmailVerificationToken { get; set; }
    public DateTime? EmailVerificationTokenExpiry { get; set; }
    public IList<Registration>? Registrations { get; init; }
}