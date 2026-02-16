namespace ObstaRace.Infrastructure.Configuration;

public class ReminderSettings
{
    public int RunAtHour { get; set; } = 9;
    public int RunAtMinute { get; set; } = 0;
    public int DaysBefore { get; set; } = 7;
}