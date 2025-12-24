using ObstaRace.API.Models;

namespace ObstaRace.API.Dto;

public class ObstacleDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Difficulty Difficulty { get; set; }
}