namespace ObstaRace.Infrastructure.Configuration;

public sealed record EmailSettings
{
    public string SmtpServer { get; init; } = string.Empty;
    public int SmtpPort { get; init; }
    public string SenderName { get; init; } = string.Empty;
    public string SenderEmail { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}