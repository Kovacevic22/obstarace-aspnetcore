using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObstaRace.Domain.Models;
public enum OrganiserStatus { Pending = 0, Approved = 1, Rejected = 2 }
public class Organiser
{
    [Key, ForeignKey("User")]
    public int UserId { get; init; }

    public User User { get; init; } = null!;

    public string OrganisationName { get; init; } = null!;
    public string Description { get; init; } = null!;
    public OrganiserStatus Status { get; set; }
}