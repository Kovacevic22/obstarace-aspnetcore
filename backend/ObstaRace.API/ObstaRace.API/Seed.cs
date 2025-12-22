using ObstaRace.API.Data;
using ObstaRace.API.Models;

namespace ObstaRace.API;

public class Seed
{
    private readonly DataContext _context;
    public Seed(DataContext context)
    {
        _context = context;
    }

    public void SeedData()
    {
       if (!_context.Races.Any())
            {
                var obstacles = new List<Obstacle>()
                {
                    new Obstacle { Name = "The Great Wall", Description = "A 10ft wooden wall", Difficulty = Difficulty.Normal },
                    new Obstacle { Name = "Mud Crawl", Description = "Crawl under barbed wire in mud", Difficulty = Difficulty.Easy },
                    new Obstacle { Name = "Monkey Bars", Description = "Slippery bars over water", Difficulty = Difficulty.Hard }
                };

                var races = new List<Race>()
                {
                    new Race
                    {
                        Name = "Summer Sludge 2024",
                        Slug = "summer-sludge-2024",
                        Date = new DateTime(2024, 07, 15),
                        Description = "The muddiest race of the year!",
                        Location = "Peak District",
                        Distance = 10.5,
                        Difficulty = Difficulty.Normal,
                        RegistrationDeadLine = new DateTime(2024, 07, 01),
                        Status = Status.UpComing,
                        ElevationGain = 450,
                        MaxParticipants = 500,
                        RaceObstacles = new List<RaceObstacle>()
                        {
                            new RaceObstacle { Obstacle = obstacles[0] },
                            new RaceObstacle { Obstacle = obstacles[1] }
                        }
                    },
                    new Race
                    {
                        Name = "Titan Trail",
                        Slug = "titan-trail",
                        Date = new DateTime(2024, 09, 20),
                        Description = "Extreme elevation and technical obstacles.",
                        Location = "Snowdonia",
                        Distance = 21.0,
                        Difficulty = Difficulty.Hard,
                        RegistrationDeadLine = new DateTime(2024, 09, 01),
                        Status = Status.UpComing,
                        ElevationGain = 1200,
                        MaxParticipants = 250,
                        RaceObstacles = new List<RaceObstacle>()
                        {
                            new RaceObstacle { Obstacle = obstacles[0] },
                            new RaceObstacle { Obstacle = obstacles[2] }
                        }
                    }
                };

                _context.Obstacles.AddRange(obstacles);
                _context.Races.AddRange(races);
                _context.SaveChanges();
            }
        }
}