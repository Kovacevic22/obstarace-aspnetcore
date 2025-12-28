namespace ObstaRace.API.Models;
public enum Category
{
    Amateur = 0,
    Advanced = 1,
    Elite = 2
}

public enum Status
{
    UpComing =0,
    OnGoing = 1,
    Completed = 2,
    Cancelled = 3,
}
public class Registration
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RaceId { get; set; }
    public User User { get; set; }
    public Race Race { get; set; }
    public string BibNumber { get; set; }
    public int Count { get; set; } = 0;
    public Category  Category { get; set; }
    public Status Status { get; set; }
}