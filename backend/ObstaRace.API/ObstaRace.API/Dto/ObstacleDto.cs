using System.ComponentModel.DataAnnotations;
using ObstaRace.API.Models;

namespace ObstaRace.API.Dto;

public class ObstacleDto
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    [Required]
    public Difficulty Difficulty { get; set; }
}

public class CreateObstacleDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Difficulty Difficulty { get; set; }
}

public class UpdateObstacleDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Difficulty Difficulty { get; set; }
}