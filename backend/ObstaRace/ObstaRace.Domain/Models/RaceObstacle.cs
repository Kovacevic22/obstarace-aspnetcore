namespace ObstaRace.Domain.Models;

public class RaceObstacle
{
    public int RaceId { get; init; }
    public int ObstacleId { get; init; }
    public Race Race { get; init; } = null!;
    public Obstacle? Obstacle { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}