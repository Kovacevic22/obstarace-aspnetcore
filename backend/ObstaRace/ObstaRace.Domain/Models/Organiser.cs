using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ObstaRace.Domain.Models;
public enum OrganiserStatus { Pending = 0, Approved = 1, Rejected = 2 }
public class Organiser
{
    [Key, ForeignKey("User")]
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public string OrganisationName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public OrganiserStatus Status { get; set; }
}