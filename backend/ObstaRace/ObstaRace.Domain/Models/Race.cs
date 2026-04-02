using System.ComponentModel.DataAnnotations;

namespace ObstaRace.Domain.Models;
public enum Difficulty
{
    Easy =0,
    Normal =1,
    Hard = 2,
}
public enum Status
{
    UpComing =0,
    OnGoing = 1,
    Completed = 2,
}
public class Race
{
    public int Id { get; init; }

    [Required(ErrorMessage = "Race name is required.")]
    [MaxLength(100)]
    public string Name { get; init; } = null!;

    [Required] [MaxLength(100)] public string Slug { get; init; } = null!;
    [Required]
    public DateTime Date  { get; init; }
    [MaxLength(500, ErrorMessage = "Description is too long.(<500 characters)")]
    public string? Description { get; init; }

    [Required(ErrorMessage = "Location is required.")]
    [MaxLength(200)]
    public string Location { get; init; } = null!;
    [Required]
    [Range(0.1, 1000.0, ErrorMessage = "Distance must be between 0.1 and 1000.")]
    public double Distance { get; init; } 
    [Required(ErrorMessage = "Race difficulty is required.")]
    public Difficulty Difficulty { get; init; }
    [Required(ErrorMessage = "Registration deadline is required.")]
    public DateTime RegistrationDeadLine { get; init; }
    [Required(ErrorMessage = "Race image status is required.")]
    public Status Status { get; init; }
    [Url(ErrorMessage = "ImageUrl format is not valid.")]
    public string? ImageUrl { get; init; }
    [Range(0, 10000, ErrorMessage = "Elevation gain must be between 0 and 10000.")]
    public int ElevationGain { get; init; }
    [Required]
    [Range(1, 10000, ErrorMessage = "Max participants must be between 1 and 1000.")]
    public int MaxParticipants { get; init; }
    public int CreatedById { get; set; }
    public User CreatedBy { get; init; } = null!;
    public bool EmailsSent { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public IList<Registration> Registrations { get; init; } = new List<Registration>();
    public IList<RaceObstacle> RaceObstacles { get; set; } = new List<RaceObstacle>();
}