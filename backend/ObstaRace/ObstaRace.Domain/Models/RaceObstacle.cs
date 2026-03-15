namespace ObstaRace.Domain.Models;

public class RaceObstacle
{
    public int RaceId { get; set; }
    public int ObstacleId { get; set; }
    public Race Race { get; set; } = null!;
    public Obstacle? Obstacle { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}