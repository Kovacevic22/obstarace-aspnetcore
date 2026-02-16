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
    Cancelled = 3,
}
public class Race
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Race name is required.")]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [MaxLength(100)]
    public string Slug { get; set; }
    [Required]
    public DateTime Date  { get; set; }
    [MaxLength(500, ErrorMessage = "Description is too long.(<500 characters)")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Location is required.")]
    [MaxLength(200)]
    public string Location  { get; set; }
    [Required]
    [Range(0.1, 1000.0, ErrorMessage = "Distance must be between 0.1 and 1000.")]
    public double Distance { get; set; }
    [Required(ErrorMessage = "Race difficulty is required.")]
    public Difficulty Difficulty { get; set; }
    [Required(ErrorMessage = "Registration deadline is required.")]
    public DateTime RegistrationDeadLine { get; set; }
    [Required(ErrorMessage = "Race image status is required.")]
    public Status Status { get; set; }
    [Url(ErrorMessage = "ImageUrl format is not valid.")]
    public string ImageUrl { get; set; }
    [Range(0, 10000, ErrorMessage = "Elevation gain must be between 0 and 10000.")]
    public int ElevationGain { get; set; }
    [Required]
    [Range(1, 10000, ErrorMessage = "Max participants must be between 1 and 1000.")]
    public int MaxParticipants { get; set; }
    public int CreatedById { get; set; } 
    public User CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public IList<Registration> Registrations { get; set; }
    public IList<RaceObstacle> RaceObstacles { get; set; }
}