namespace ObstaRace.API.Models;
public enum Category
{
    Amateur,
    Advanced,
    Elite
}

public enum Status
{
    UpComing,
    OnGoing,
    Completed,
    Cancelled,
}
public class Registration
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RaceId { get; set; }
    public User User { get; set; }
    public Race Race { get; set; }
    public string BibNumber { get; set; }
    public Category  Category { get; set; }
    public Status Status { get; set; }
}