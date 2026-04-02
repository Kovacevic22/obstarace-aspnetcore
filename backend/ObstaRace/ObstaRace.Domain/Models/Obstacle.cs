using System.ComponentModel.DataAnnotations;

namespace ObstaRace.Domain.Models;

public class Obstacle
{
    public int Id { get; init; }
    [Required] [MaxLength(100)] public string Name { get; init; } = null!;
    [MaxLength(500)]
    public string? Description { get; init; }
    [Required]
    public Difficulty Difficulty { get; init; }
    public int CreatedById { get; set; }
    public User? CreatedBy { get; init; }
    public IList<RaceObstacle>? RaceObstacles { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}