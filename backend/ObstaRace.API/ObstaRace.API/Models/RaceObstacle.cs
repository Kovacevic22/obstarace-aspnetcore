namespace ObstaRace.API.Models;

public class RaceObstacle
{
    public int RaceId { get; set; }
    public int ObstacleId { get; set; }
    public Race Race { get; set; }
    public Obstacle Obstacle { get; set; }
}