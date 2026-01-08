using System.ComponentModel.DataAnnotations;

namespace ObstaRace.API.Models;

public class Obstacle
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [MaxLength(500)]
    public string Description { get; set; }
    [Required]
    public Difficulty Difficulty { get; set; }
    public int CreatedById { get; set; }
    public User CreatedBy { get; set; }
    public IList<RaceObstacle> RaceObstacles { get; set; }
}