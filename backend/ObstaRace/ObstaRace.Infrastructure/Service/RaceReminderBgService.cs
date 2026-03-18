using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ObstaRace.Application.Interfaces.Repositories;
using ObstaRace.Application.Interfaces.Services;
using ObstaRace.Infrastructure.Configuration;

namespace ObstaRace.Infrastructure.Service;

public class RaceReminderBgService:BackgroundService
{
    private readonly ILogger<RaceReminderBgService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ReminderSettings _settings;
    public RaceReminderBgService(IServiceProvider serviceProvider, ILogger<RaceReminderBgService> logger, IOptions<ReminderSettings> settings)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _settings = settings.Value;
        
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Race Reminder Background Service started");
        await DoWork(stoppingToken);
        var now = DateTime.UtcNow;
        var scheduledTime = new DateTime(
            now.Year, 
            now.Month, 
            now.Day, 
            _settings.RunAtHour,      
            _settings.RunAtMinute,
            0
        );
        if (now > scheduledTime)
        {
            scheduledTime = scheduledTime.AddDays(1);
        }
        var timeUntilFirstRun = scheduledTime - now;
        _logger.LogInformation(
            "Reminder service will run daily at {Hour}:{Minute:D2} (First run: {Time})",
            _settings.RunAtHour,
            _settings.RunAtMinute,
            scheduledTime
        );
        await Task.Delay(timeUntilFirstRun, stoppingToken);
        using var timer = new PeriodicTimer(TimeSpan.FromHours(24));
        do
        {
            await DoWork(stoppingToken);
        } 
        while (await timer.WaitForNextTickAsync(stoppingToken));
    }
    private async Task DoWork(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Race Reminder Background Service started");
        try
        {
            await SendRaceReminders();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while sending race reminders");
        }
    }

    public async Task SendRaceReminders()
    {
        using var scope = _serviceProvider.CreateScope();
        
        var registrationRepo = scope.ServiceProvider.GetRequiredService<IRegistrationRepository>();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
        
        _logger.LogInformation("Checking for races in 7 days...");
        var targetDate = DateTime.Today.AddDays(_settings.DaysBefore);
        var processedCount = 0;
        
        using var writeScope = _serviceProvider.CreateScope();
        var writeRepo = writeScope.ServiceProvider.GetRequiredService<IRegistrationRepository>();
        
        await foreach (var registration in registrationRepo.GetRegistrationsForReminderAsync(targetDate))
        {
            try
            {
                var participantName = registration.User.Participant != null
                    ? $"{registration.User.Participant.Name} {registration.User.Participant.Surname}"
                    : "Unknown name";
                await emailService.SendRaceReminderAsync(
                    recipientEmail: registration.User.Email,
                    recipientName: participantName,
                    raceName: registration.Race.Name,
                    raceDate: registration.Race.Date,
                    location: registration.Race.Location
                );
                await writeRepo.MarkReminderAsSentForRegistration(registration.Id);
                processedCount++;
                _logger.LogInformation("Reminder sent to {Email} ({Count})", registration.User.Email, processedCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send reminder for registration {RegistrationId}", registration.Id);
            }
        }
    }
}