namespace ObstaRace.API.Models;
public enum Difficulty
{
    Easy =0,
    Normal =1,
    Hard = 2,
}
public class Race
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public DateTime Date  { get; set; }
    public string Description { get; set; }
    public string Location  { get; set; }
    public double Distance { get; set; }
    public Difficulty Difficulty { get; set; }
    public DateTime RegistrationDeadLine { get; set; }
    public Status Status { get; set; }
    public string ImageUrl { get; set; }
    public int ElevationGain { get; set; }
    public int MaxParticipants { get; set; }
    public IList<Registration> Registrations { get; set; }
    public IList<RaceObstacle> RaceObstacles { get; set; }
}