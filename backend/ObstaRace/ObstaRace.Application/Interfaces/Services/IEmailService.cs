namespace ObstaRace.Application.Interfaces.Services;

public interface IEmailService
{
    Task SendRaceRegistrationConfirmationAsync(string recipientEmail, string recipientName, string raceName, DateTime raceDate, string location);
    Task SendGenericEmailAsync(string recipientEmail, string subject, string htmlBody);
    Task SendRaceRegistrationApprovedAsync(string recipientEmail, string recipientName, string raceName);
    Task SendRaceRegistrationRejectedAsync(string recipientEmail, string recipientName, string raceName, string reason);

    Task SendRaceReminderAsync(string recipientEmail, string recipientName, string raceName, DateTime raceDate, string location);
}