using System.Reflection;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using ObstaRace.Application.Interfaces.Services;
using ObstaRace.Infrastructure.Configuration;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace ObstaRace.Infrastructure.Service;

public class EmailService:IEmailService
{
    private readonly EmailSettings _emailSettings;
    private ILogger<EmailService> _logger;
    private readonly IConfiguration _config;
    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger, IConfiguration config)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
        _config = config;
    }
    public async Task SendRaceRegistrationConfirmationAsync(string recipientEmail, string recipientName, string raceName, DateTime raceDate, string location)
    {
        var subject = $"Race Registration Confirmation: {raceName}";
        var htmlBody = await LoadTemplateAsync("RaceRegistrationConfirmed.html");
        htmlBody = htmlBody
            .Replace("{{RecipientName}}", recipientName)
            .Replace("{{RaceName}}", raceName)
            .Replace("{{RaceDate}}", raceDate.ToString("dd/MM/yyyy - HH:mm"))
            .Replace("{{Location}}", location);
        await SendEmailAsync(recipientEmail, subject, htmlBody);
    }
    public async Task SendRaceRegistrationApprovedAsync(string recipientEmail, string recipientName, string raceName)
    {
        var subject = $"Registration Approved: {raceName}";
        var htmlBody = await LoadTemplateAsync("RaceRegistrationApproved.html");
        var frontendUrl = _config["FrontendSettings:BaseUrl"];
        htmlBody = htmlBody
            .Replace("{{RecipientName}}", recipientName)
            .Replace("{{RaceName}}", raceName)
            .Replace("{{FrontendUrl}}", frontendUrl);

        await SendEmailAsync(recipientEmail, subject, htmlBody);
    }

    public async Task SendRaceRegistrationRejectedAsync(string recipientEmail, string recipientName, string raceName, string reason)
    {
        var subject = $"Registration Update: {raceName}";
        var htmlBody = await LoadTemplateAsync("RaceRegistrationRejected.html");
        var frontendUrl = _config["FrontendSettings:BaseUrl"];
        htmlBody = htmlBody
            .Replace("{{RecipientName}}", recipientName)
            .Replace("{{RaceName}}", raceName)
            .Replace("{{Reason}}", reason)
            .Replace("{{FrontendUrl}}", frontendUrl);

        await SendEmailAsync(recipientEmail, subject, htmlBody);
    }

    public async Task SendRaceCompletedAsync(string recipientEmail, string recipientName, string raceName, DateTime raceDate, string location)
    {
        var subject = $"Congratulations! {raceName} Completed";
        var htmlBody = await LoadTemplateAsync("RaceCompleted.html");
        htmlBody = htmlBody
            .Replace("{{RecipientName}}", recipientName)
            .Replace("{{RaceName}}", raceName)
            .Replace("{{RaceDate}}", raceDate.ToString("dddd, MMMM dd, yyyy"))
            .Replace("{{Location}}", location);

        await SendEmailAsync(recipientEmail, subject, htmlBody);
    }

    public async Task SendRaceReminderAsync(string recipientEmail, string recipientName, string raceName, DateTime raceDate, string location)
    {
        var subject = $"Reminder: {raceName}";
        var htmlBody = await LoadTemplateAsync("RaceReminder.html");
        htmlBody = htmlBody
            .Replace("{{RecipientName}}", recipientName)
            .Replace("{{RaceName}}", raceName)
            .Replace("{{Location}}", location)
            .Replace("{{RaceDate}}", raceDate.ToString("dd/MM/yyyy - HH:mm"));
        await SendEmailAsync(recipientEmail, subject, htmlBody);
    }
    public async Task SendGenericEmailAsync(string recipientEmail, string subject, string htmlBody)
    {
        await SendEmailAsync(recipientEmail, subject, htmlBody);
    }
    private async Task<string> LoadTemplateAsync(string templateName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"ObstaRace.Infrastructure.EmailTemplates.{templateName}";
        await using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            _logger.LogError("Email template not found: {templateName}", templateName);
            throw new FileNotFoundException($"Email template not found: {templateName}");
        }
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
    private async Task SendEmailAsync(string recipientEmail, string subject, string htmlBody)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
        message.To.Add(new MailboxAddress("", recipientEmail));
        message.Subject = subject;
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlBody,
        };
        message.Body = bodyBuilder.ToMessageBody();
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
            await client.SendAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to send email: {message} ", ex.Message);
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}