namespace ObstaRace.API.Models;

public class Obstacle
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Difficulty Difficulty { get; set; }
    public IList<RaceObstacle> RaceObstacles { get; set; }
}