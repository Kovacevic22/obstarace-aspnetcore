using Microsoft.EntityFrameworkCore;
using ObstaRace.Domain.Models;
using ObstaRace.Infrastructure.Data;

namespace ObstaRace.Infrastructure.Seeders;

public class DataSeeder
{
    public static async Task SeedAsync(DataContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Users.AnyAsync()) return;

        var passwordHash = BCrypt.Net.BCrypt.HashPassword("Password123!");

        var admin = new User
        {
            Email = "admin@obstarace.com",
            PasswordHash = passwordHash,
            PhoneNumber = "+381601111111",
            Role = Role.Admin,
        };

        var organiser1 = new User
        {
            Email = "organiser1@obstarace.com",
            PasswordHash = passwordHash,
            PhoneNumber = "+381602222222",
            Role = Role.Organiser,
        };

        var organiser2 = new User
        {
            Email = "organiser2@obstarace.com",
            PasswordHash = passwordHash,
            PhoneNumber = "+381603333333",
            Role = Role.Organiser,
        };

        var userList = new List<User>
        {
            new() { Email = "marko.petrovic@gmail.com",    PasswordHash = passwordHash, PhoneNumber = "+381641000001", Role = Role.User },
            new() { Email = "ana.jovanovic@gmail.com",     PasswordHash = passwordHash, PhoneNumber = "+381641000002", Role = Role.User },
            new() { Email = "nikola.nikolic@gmail.com",    PasswordHash = passwordHash, PhoneNumber = "+381641000003", Role = Role.User },
            new() { Email = "milica.stanic@gmail.com",     PasswordHash = passwordHash, PhoneNumber = "+381641000004", Role = Role.User },
            new() { Email = "stefan.djordjevic@gmail.com", PasswordHash = passwordHash, PhoneNumber = "+381641000005", Role = Role.User },
        };

        await context.Users.AddAsync(admin);
        await context.Users.AddAsync(organiser1);
        await context.Users.AddAsync(organiser2);
        await context.Users.AddRangeAsync(userList);
        await context.SaveChangesAsync();

        await context.Organisers.AddRangeAsync(
            new Organiser
            {
                UserId = organiser1.Id,
                OrganisationName = "AdventureRace Serbia",
                Description = "Organizujemo najludje obstacle race trke u Srbiji od 2018.",
                Status = OrganiserStatus.Approved,
            },
            new Organiser
            {
                UserId = organiser2.Id,
                OrganisationName = "MudRun Balkans",
                Description = "Specijalizovani za blato i ekstremne outdoor takmičenja.",
                Status = OrganiserStatus.Approved,
            }
        );
        await context.SaveChangesAsync();

        var participantData = new (string Name, string Surname, DateTime Dob, string Emergency)[]
        {
            ("Marko",  "Petrović",  new DateTime(1995, 3,  12), "+381641111111"),
            ("Ana",    "Jovanović", new DateTime(1998, 7,  24), "+381641111112"),
            ("Nikola", "Nikolić",   new DateTime(1993, 11,  5), "+381641111113"),
            ("Milica", "Stanić",    new DateTime(2000, 1,  18), "+381641111114"),
            ("Stefan", "Đorđević",  new DateTime(1997, 6,  30), "+381641111115"),
        };

        await context.Participants.AddRangeAsync(
            userList.Select((u, i) => new Participant
            {
                UserId = u.Id,
                Name = participantData[i].Name,
                Surname = participantData[i].Surname,
                DateOfBirth = participantData[i].Dob,
                EmergencyContact = participantData[i].Emergency,
            })
        );
        await context.SaveChangesAsync();

        var obstacles = new List<Obstacle>
        {
            new() { Name = "Mud Pit",        Description = "Prolaz kroz duboko blato ispod bodljikave žice.", Difficulty = Difficulty.Easy,   CreatedById = organiser1.Id },
            new() { Name = "Rope Climb",     Description = "Penjanje na konop visine 6 metara.",              Difficulty = Difficulty.Normal, CreatedById = organiser1.Id },
            new() { Name = "Monkey Bars",    Description = "Prelaz po horizontalnim šipkama iznad vode.",     Difficulty = Difficulty.Normal, CreatedById = organiser1.Id },
            new() { Name = "Wall Climb",     Description = "Prelaz preko zida visine 4 metra.",               Difficulty = Difficulty.Hard,   CreatedById = organiser1.Id },
            new() { Name = "Tire Flip",      Description = "Prevrtanje traktorske gume 10 puta.",             Difficulty = Difficulty.Hard,   CreatedById = organiser2.Id },
            new() { Name = "Cargo Net",      Description = "Penjanje i spuštanje po mrežastoj prepreci.",     Difficulty = Difficulty.Easy,   CreatedById = organiser2.Id },
            new() { Name = "Ice Bath",       Description = "Prolaz kroz ledenu vodu.",                        Difficulty = Difficulty.Hard,   CreatedById = organiser2.Id },
            new() { Name = "Sandbag Carry",  Description = "Nošenje vreće peska od 25kg na 200 metara.",      Difficulty = Difficulty.Normal, CreatedById = organiser2.Id },
            new() { Name = "Balance Beam",   Description = "Prelaz po uskoj gredi iznad blata.",              Difficulty = Difficulty.Easy,   CreatedById = organiser1.Id },
            new() { Name = "Fire Jump",      Description = "Preskok vatrene prepreke na cilju.",              Difficulty = Difficulty.Normal, CreatedById = organiser1.Id },
            new() { Name = "Spear Throw",    Description = "Bacanje koplja u metu sa 5 metara.",              Difficulty = Difficulty.Normal, CreatedById = organiser2.Id },
            new() { Name = "Barbed Wire",    Description = "Puzanje ispod bodljikave žice na 30 metara.",     Difficulty = Difficulty.Easy,   CreatedById = organiser2.Id },
            new() { Name = "Atlas Stone",    Description = "Nošenje kamene kugle od 50kg na 20 metara.",      Difficulty = Difficulty.Hard,   CreatedById = organiser1.Id },
            new() { Name = "Swim Section",   Description = "Plivanje kroz vodenu prepreku dužine 50m.",       Difficulty = Difficulty.Normal, CreatedById = organiser1.Id },
            new() { Name = "Hanging Rings",  Description = "Prelaz na karikama iznad vodene jame.",           Difficulty = Difficulty.Hard,   CreatedById = organiser2.Id },
        };

        await context.Obstacles.AddRangeAsync(obstacles);
        await context.SaveChangesAsync();

        var races = new List<Race>
        {
            new()
            {
                Name = "Balkan Beast 2024",
                Slug = "balkan-beast-2024",
                Date = new DateTime(2024, 9, 14),
                RegistrationDeadLine = new DateTime(2024, 9, 1),
                Description = "Najtežja obstacle race trka na Balkanu. 15km kroz planinski teren sa 20+ prepreka.",
                Location = "Kopaonik, Srbija",
                Distance = 15.0,
                Difficulty = Difficulty.Hard,
                ElevationGain = 850,
                MaxParticipants = 500,
                Status = Status.Completed,
                ImageUrl = "https://images.unsplash.com/photo-1533560904424-a0c61dc306fc",
                CreatedById = organiser1.Id,
            },
            new()
            {
                Name = "Mud Run Novi Sad 2024",
                Slug = "mud-run-novi-sad-2024",
                Date = new DateTime(2024, 10, 19),
                RegistrationDeadLine = new DateTime(2024, 10, 10),
                Description = "Klasična mud run trka za početnike i iskusne trkače. 8km kroz ravničarski teren.",
                Location = "Fruška Gora, Novi Sad",
                Distance = 8.0,
                Difficulty = Difficulty.Normal,
                ElevationGain = 200,
                MaxParticipants = 300,
                Status = Status.Completed,
                ImageUrl = "https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b",
                CreatedById = organiser2.Id,
            },
            new()
            {
                Name = "Winter Warrior 2024",
                Slug = "winter-warrior-2024",
                Date = new DateTime(2024, 12, 7),
                RegistrationDeadLine = new DateTime(2024, 11, 25),
                Description = "Zimska obstacle trka na snegu. 12km kroz planinski teren sa ledenim preprekama.",
                Location = "Zlatibor, Srbija",
                Distance = 12.0,
                Difficulty = Difficulty.Hard,
                ElevationGain = 600,
                MaxParticipants = 250,
                Status = Status.Completed,
                ImageUrl = "https://images.unsplash.com/photo-1551698618-1dfe5d97d256",
                CreatedById = organiser1.Id,
            },
            new()
            {
                Name = "Spring Sprint Beograd 2025",
                Slug = "spring-sprint-beograd-2025",
                Date = new DateTime(2025, 4, 12),
                RegistrationDeadLine = new DateTime(2025, 4, 1),
                Description = "Prolećna sprint trka kroz parkove Beograda. 5km sa 10 prepreka.",
                Location = "Kalemegdan, Beograd",
                Distance = 5.0,
                Difficulty = Difficulty.Easy,
                ElevationGain = 80,
                MaxParticipants = 400,
                Status = Status.Completed,
                ImageUrl = "https://images.unsplash.com/photo-1544367567-0f2fcb009e0b",
                CreatedById = organiser2.Id,
            },
            new()
            {
                Name = "Balkan Beast 2025",
                Slug = "balkan-beast-2025",
                Date = new DateTime(2025, 9, 15),
                RegistrationDeadLine = new DateTime(2025, 9, 1),
                Description = "Najtežja obstacle race trka na Balkanu. 15km kroz planinski teren sa 20+ prepreka.",
                Location = "Kopaonik, Srbija",
                Distance = 15.0,
                Difficulty = Difficulty.Hard,
                ElevationGain = 850,
                MaxParticipants = 500,
                Status = Status.Completed,
                ImageUrl = "https://images.unsplash.com/photo-1533560904424-a0c61dc306fc",
                CreatedById = organiser1.Id,
            },
            new()
            {
                Name = "Mud Run Novi Sad 2025",
                Slug = "mud-run-novi-sad-2025",
                Date = new DateTime(2025, 10, 20),
                RegistrationDeadLine = new DateTime(2025, 10, 10),
                Description = "Klasična mud run trka za početnike i iskusne trkače. 8km kroz ravničarski teren.",
                Location = "Fruška Gora, Novi Sad",
                Distance = 8.0,
                Difficulty = Difficulty.Normal,
                ElevationGain = 200,
                MaxParticipants = 300,
                Status = Status.Completed,
                ImageUrl = "https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b",
                CreatedById = organiser2.Id,
            },
            new()
            {
                Name = "Danube Run Challenge",
                Slug = "danube-run-challenge-2025",
                Date = new DateTime(2025, 6, 21),
                RegistrationDeadLine = new DateTime(2025, 6, 10),
                Description = "Trka duž obale Dunava sa vodenim preprekama. 10km ravničarskog terena.",
                Location = "Novi Sad, Vojvodina",
                Distance = 10.0,
                Difficulty = Difficulty.Normal,
                ElevationGain = 50,
                MaxParticipants = 350,
                Status = Status.Completed,
                ImageUrl = "https://images.unsplash.com/photo-1526976668912-1a811878dd37",
                CreatedById = organiser1.Id,
            },
            new()
            {
                Name = "Šar Planina Ultra",
                Slug = "sar-planina-ultra-2025",
                Date = new DateTime(2025, 7, 19),
                RegistrationDeadLine = new DateTime(2025, 7, 5),
                Description = "Ultra obstacle trka na Šar planini. 25km sa 30+ prepreka i 1200m uspona.",
                Location = "Šar Planina, Kosovo",
                Distance = 25.0,
                Difficulty = Difficulty.Hard,
                ElevationGain = 1200,
                MaxParticipants = 150,
                Status = Status.Completed,
                ImageUrl = "https://images.unsplash.com/photo-1551698618-1dfe5d97d256",
                CreatedById = organiser2.Id,
            },
            new()
            {
                Name = "Beginner Blast Niš",
                Slug = "beginner-blast-nis-2025",
                Date = new DateTime(2025, 5, 17),
                RegistrationDeadLine = new DateTime(2025, 5, 5),
                Description = "Savršena trka za prve korake u obstacle racing svetu. 3km sa lakim preprekama.",
                Location = "Niš, Srbija",
                Distance = 3.0,
                Difficulty = Difficulty.Easy,
                ElevationGain = 30,
                MaxParticipants = 500,
                Status = Status.Completed,
                ImageUrl = "https://images.unsplash.com/photo-1544367567-0f2fcb009e0b",
                CreatedById = organiser1.Id,
            },
            new()
            {
                Name = "Tara Forest Race",
                Slug = "tara-forest-race-2025",
                Date = new DateTime(2025, 8, 23),
                RegistrationDeadLine = new DateTime(2025, 8, 10),
                Description = "Trka kroz gustу šumu nacionalnog parka Tara. 18km sa 15 prepreka.",
                Location = "NP Tara, Srbija",
                Distance = 18.0,
                Difficulty = Difficulty.Hard,
                ElevationGain = 750,
                MaxParticipants = 200,
                Status = Status.Completed,
                ImageUrl = "https://images.unsplash.com/photo-1533560904424-a0c61dc306fc",
                CreatedById = organiser2.Id,
            },
            new()
            {
                Name = "ObstaRace Spring 2026",
                Slug = "obstarace-spring-2026",
                Date = new DateTime(2026, 4, 18),
                RegistrationDeadLine = new DateTime(2026, 4, 5),
                Description = "Prolećna obstacle trka na Avali. 10km sa 15 prepreka raznih težina.",
                Location = "Avala, Beograd",
                Distance = 10.0,
                Difficulty = Difficulty.Normal,
                ElevationGain = 400,
                MaxParticipants = 400,
                Status = Status.UpComing,
                ImageUrl = "https://images.unsplash.com/photo-1544367567-0f2fcb009e0b",
                CreatedById = organiser1.Id,
            },
            new()
            {
                Name = "Summer Blast 2026",
                Slug = "summer-blast-2026",
                Date = new DateTime(2026, 7, 5),
                RegistrationDeadLine = new DateTime(2026, 6, 25),
                Description = "Letnja trka sa vodenim preprekama. 12km uz reku Savu.",
                Location = "Ada Ciganlija, Beograd",
                Distance = 12.0,
                Difficulty = Difficulty.Hard,
                ElevationGain = 300,
                MaxParticipants = 350,
                Status = Status.UpComing,
                ImageUrl = "https://images.unsplash.com/photo-1526976668912-1a811878dd37",
                CreatedById = organiser2.Id,
            },
            new()
            {
                Name = "Balkan Beast 2026",
                Slug = "balkan-beast-2026",
                Date = new DateTime(2026, 9, 12),
                RegistrationDeadLine = new DateTime(2026, 9, 1),
                Description = "Treće izdanje najtežje obstacle race trke na Balkanu. 20km sa 25+ prepreka.",
                Location = "Kopaonik, Srbija",
                Distance = 20.0,
                Difficulty = Difficulty.Hard,
                ElevationGain = 950,
                MaxParticipants = 600,
                Status = Status.UpComing,
                ImageUrl = "https://images.unsplash.com/photo-1533560904424-a0c61dc306fc",
                CreatedById = organiser1.Id,
            },
            new()
            {
                Name = "Night Race Zlatibor",
                Slug = "night-race-zlatibor-2026",
                Date = new DateTime(2026, 8, 8),
                RegistrationDeadLine = new DateTime(2026, 7, 25),
                Description = "Noćna obstacle trka na Zlatiboru. 8km sa preprekama osvetljenim UV svetlom.",
                Location = "Zlatibor, Srbija",
                Distance = 8.0,
                Difficulty = Difficulty.Normal,
                ElevationGain = 350,
                MaxParticipants = 300,
                Status = Status.UpComing,
                ImageUrl = "https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b",
                CreatedById = organiser2.Id,
            },
            new()
            {
                Name = "City Obstacle Tour Beograd",
                Slug = "city-obstacle-tour-beograd-2026",
                Date = new DateTime(2026, 5, 9),
                RegistrationDeadLine = new DateTime(2026, 4, 28),
                Description = "Urbana obstacle trka kroz centar Beograda. 6km sa 12 prepreka.",
                Location = "Centar, Beograd",
                Distance = 6.0,
                Difficulty = Difficulty.Easy,
                ElevationGain = 60,
                MaxParticipants = 800,
                Status = Status.UpComing,
                ImageUrl = "https://images.unsplash.com/photo-1544367567-0f2fcb009e0b",
                CreatedById = organiser1.Id,
            },
            new()
            {
                Name = "Rtanj Mountain Challenge",
                Slug = "rtanj-mountain-challenge-2026",
                Date = new DateTime(2026, 6, 13),
                RegistrationDeadLine = new DateTime(2026, 6, 1),
                Description = "Obstacle trka na misterioznoj planini Rtanj. 14km sa 18 prepreka.",
                Location = "Rtanj, Srbija",
                Distance = 14.0,
                Difficulty = Difficulty.Hard,
                ElevationGain = 700,
                MaxParticipants = 200,
                Status = Status.UpComing,
                ImageUrl = "https://images.unsplash.com/photo-1551698618-1dfe5d97d256",
                CreatedById = organiser2.Id,
            },
            new()
            {
                Name = "Fruška Gora Trail 2026",
                Slug = "fruska-gora-trail-2026",
                Date = new DateTime(2026, 10, 3),
                RegistrationDeadLine = new DateTime(2026, 9, 20),
                Description = "Jesenja obstacle trka kroz vinograde Fruške Gore. 11km sa 13 prepreka.",
                Location = "Fruška Gora, Vojvodina",
                Distance = 11.0,
                Difficulty = Difficulty.Normal,
                ElevationGain = 280,
                MaxParticipants = 320,
                Status = Status.UpComing,
                ImageUrl = "https://images.unsplash.com/photo-1526976668912-1a811878dd37",
                CreatedById = organiser1.Id,
            },
            new()
            {
                Name = "Đerdap Gorge Race",
                Slug = "derdap-gorge-race-2026",
                Date = new DateTime(2026, 9, 26),
                RegistrationDeadLine = new DateTime(2026, 9, 12),
                Description = "Spektakularna trka uz obalu Đerdapske klisure. 16km sa 20 prepreka.",
                Location = "NP Đerdap, Srbija",
                Distance = 16.0,
                Difficulty = Difficulty.Hard,
                ElevationGain = 500,
                MaxParticipants = 180,
                Status = Status.UpComing,
                ImageUrl = "https://images.unsplash.com/photo-1533560904424-a0c61dc306fc",
                CreatedById = organiser2.Id,
            },
            new()
            {
                Name = "Rookie Run Kragujevac",
                Slug = "rookie-run-kragujevac-2026",
                Date = new DateTime(2026, 5, 23),
                RegistrationDeadLine = new DateTime(2026, 5, 12),
                Description = "Idealna trka za početnike u Šumadiji. 4km sa 8 lakih prepreka.",
                Location = "Kragujevac, Srbija",
                Distance = 4.0,
                Difficulty = Difficulty.Easy,
                ElevationGain = 40,
                MaxParticipants = 600,
                Status = Status.UpComing,
                ImageUrl = "https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b",
                CreatedById = organiser1.Id,
            },
            new()
            {
                Name = "Stara Planina Winter Race",
                Slug = "stara-planina-winter-race-2026",
                Date = new DateTime(2026, 12, 5),
                RegistrationDeadLine = new DateTime(2026, 11, 22),
                Description = "Zimska obstacle trka na Staroj Planini. 10km u snegu i ledu.",
                Location = "Stara Planina, Srbija",
                Distance = 10.0,
                Difficulty = Difficulty.Hard,
                ElevationGain = 550,
                MaxParticipants = 220,
                Status = Status.UpComing,
                ImageUrl = "https://images.unsplash.com/photo-1551698618-1dfe5d97d256",
                CreatedById = organiser2.Id,
            },
        };

        await context.Races.AddRangeAsync(races);
        await context.SaveChangesAsync();

        var raceObstacles = new List<RaceObstacle>();

        var hardObstacles = new[] { 0, 1, 3, 4, 6, 9, 12, 14 };
        var normalObstacles = new[] { 0, 2, 5, 7, 9, 10, 13 };
        var easyObstacles = new[] { 0, 5, 8, 9, 11 };

        foreach (var race in races)
        {
            var obstacleSet = race.Difficulty switch
            {
                Difficulty.Hard => hardObstacles,
                Difficulty.Normal => normalObstacles,
                _ => easyObstacles
            };

            foreach (var i in obstacleSet)
            {
                if (i < obstacles.Count)
                {
                    raceObstacles.Add(new RaceObstacle
                    {
                        RaceId = race.Id,
                        ObstacleId = obstacles[i].Id
                    });
                }
            }
        }

        await context.RaceObstacles.AddRangeAsync(raceObstacles);
        await context.SaveChangesAsync();
    }
}