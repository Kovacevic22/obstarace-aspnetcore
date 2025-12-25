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
        if (!_context.Users.Any())
        {
            var users = new List<User>()
            {
                new User
                {
                    Name = "John",
                    Surname = "Doe",
                    Email = "john.doe@example.com",
                    PasswordHash = "password123", 
                    PhoneNumber = "+44 7700 900000",
                    DateOfBirth = new DateTime(1990, 5, 15),
                    EmergencyContact = "+44 7700 900001",
                    Role = Role.User
                },
                new User
                {
                    Name = "Jane",
                    Surname = "Smith",
                    Email = "jane.smith@example.com",
                    PasswordHash = "password123",
                    PhoneNumber = "+44 7700 900002",
                    DateOfBirth = new DateTime(1992, 8, 22),
                    EmergencyContact = "+44 7700 900003",
                    Role = Role.User
                },
                new User
                {
                    Name = "Admin",
                    Surname = "User",
                    Email = "admin@obstarace.com",
                    PasswordHash = "admin123",
                    PhoneNumber = "+44 7700 900004",
                    DateOfBirth = new DateTime(1985, 3, 10),
                    EmergencyContact = "+44 7700 900005",
                    Role = Role.Admin
                },
                new User
                {
                    Name = "Mike",
                    Surname = "Organizer",
                    Email = "mike.organizer@obstarace.com",
                    PasswordHash = "organizer123",
                    PhoneNumber = "+44 7700 900006",
                    DateOfBirth = new DateTime(1988, 11, 5),
                    EmergencyContact = "+44 7700 900007",
                    Role = Role.Organizer
                },
                new User
                {
                    Name = "Sarah",
                    Surname = "Johnson",
                    Email = "sarah.johnson@example.com",
                    PasswordHash = "password123",
                    PhoneNumber = "+44 7700 900008",
                    DateOfBirth = new DateTime(1995, 1, 30),
                    EmergencyContact = "+44 7700 900009",
                    Role = Role.User
                }
            };

            _context.Users.AddRange(users);
            _context.SaveChanges();
        }
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