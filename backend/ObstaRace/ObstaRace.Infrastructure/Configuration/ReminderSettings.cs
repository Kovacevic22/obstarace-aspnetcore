namespace ObstaRace.Infrastructure.Configuration;

public sealed record ReminderSettings
{
    public int RunAtHour { get; init; } = 9;
    public int RunAtMinute { get; init; } = 0;
    public int DaysBefore { get; init; } = 7;
}