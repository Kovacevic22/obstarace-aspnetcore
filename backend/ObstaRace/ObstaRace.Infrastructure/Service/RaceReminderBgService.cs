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
    private Timer? _timer;
    private readonly ReminderSettings _settings;
    public RaceReminderBgService(IServiceProvider serviceProvider, ILogger<RaceReminderBgService> logger, IOptions<ReminderSettings> settings)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _settings = settings.Value;
        
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Race Reminder Background Service started");
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
        _timer = new Timer(
            DoWork,
            null,
            timeUntilFirstRun,
            TimeSpan.FromHours(24)
            );
        return Task.CompletedTask;  
    }
    // protected override Task ExecuteAsync(CancellationToken stoppingToken) //TEST MODE
    // {
    //     _timer = new Timer(
    //         DoWork,
    //         null,
    //         TimeSpan.FromSeconds(10),   
    //         TimeSpan.FromMinutes(1) 
    //     );
    //     return Task.CompletedTask;
    // }
    private async void DoWork(object? obj)
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
        var targetDate = DateTime.Today.AddDays(7);
        var processedCount = 0;
        await foreach (var registration in registrationRepo.GetRegistrationsForReminderAsync(targetDate))
        {
            try
            {
                var participantName = registration.User.Participant != null
                    ? $"{registration.User.Participant.Name} {registration.Participant.Surname}"
                    : "Unknown name";
                await emailService.SendRaceReminderAsync(
                    recipientEmail: registration.User.Email,
                    recipientName: participantName,
                    raceName: registration.Race.Name,
                    raceDate: registration.Race.Date,
                    location: registration.Race.Location
                );
                registration.ReminderSent = true;
                await registrationRepo.UpdateRegistration(registration);
                processedCount++;
                _logger.LogInformation("Reminder sent to {Email} ({Count})", registration.User.Email, processedCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send reminder for registration {RegistrationId}", registration.Id);
            }
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Race Reminder Background Service is stopping");
        _timer?.Change(Timeout.Infinite, 0);
        return base.StopAsync(cancellationToken);
    }
    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}