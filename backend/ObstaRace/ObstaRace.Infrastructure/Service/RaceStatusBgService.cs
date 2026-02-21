using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ObstaRace.Application.Interfaces.Repositories;
using ObstaRace.Application.Interfaces.Services;
using ObstaRace.Domain.Models;

namespace ObstaRace.Infrastructure.Service;

public class RaceStatusBgService:BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RaceStatusBgService> _logger;
    private Timer? _timer;
    public RaceStatusBgService(IServiceProvider serviceProvider, ILogger<RaceStatusBgService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Race Status Background Service started");
        _timer = new Timer(
            DoWork,
            null,
            TimeSpan.Zero,           
            TimeSpan.FromHours(1)  
        );
        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        _logger.LogInformation("Checking race statuses at {Time}", DateTime.UtcNow);
        try
        {
            await UpdateRaceStatuses();
            await FinishCompletedRaces();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error updating race statuses");
        }
    }

    private async Task UpdateRaceStatuses()
    {
        using var scope = _serviceProvider.CreateScope();
        var raceRepo = scope.ServiceProvider.GetRequiredService<IRaceRepository>();
        var raceToStart = await raceRepo.GetRacesStartingToday();
        foreach (var race in raceToStart)
        {
            await raceRepo.UpdateRaceStatus(race.Id, Status.OnGoing);
            _logger.LogInformation("Race {RaceName} marked as OnGoing", race.Name);
        }
        var racesToComplete = await raceRepo.GetRacesToComplete();
        foreach (var race in racesToComplete)
        {
            await raceRepo.UpdateRaceStatus(race.Id, Status.Completed);
            _logger.LogInformation("Race {RaceName} marked as Completed", race.Name);
        }
    }

    private async Task FinishCompletedRaces()
    {
        using var scope = _serviceProvider.CreateScope();
        var raceRepo = scope.ServiceProvider.GetRequiredService<IRaceRepository>();
        var registrationRepo = scope.ServiceProvider.GetRequiredService<IRegistrationRepository>();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
        var completedRaces = await raceRepo.GetCompletedRaces();
        _logger.LogInformation("Found {Count} completed races with pending registrations", completedRaces.Count);
        var totalProcessed = 0;
        foreach (var race in completedRaces)
        {
            await foreach (var registration in registrationRepo.GetRegistrationsForCompletedRace(race.Id))
            {
                try
                {
                    registration.Status = RegistrationStatus.Finished;
                    await registrationRepo.UpdateRegistration(registration);
                    var participantName = registration.User.Participant != null
                        ? $"{registration.User.Participant.Name} {registration.User.Participant.Surname}"
                        : "Unknown name";
                    await emailService.SendRaceCompletedAsync(
                        recipientEmail: registration.User.Email,
                        recipientName: participantName,
                        raceName: race.Name,
                        raceDate: race.Date,
                        location: race.Location
                    );
                    totalProcessed++;
                    _logger.LogInformation("Finished registration for {Email} ({Count})", registration.User.Email, totalProcessed);
                }catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to finish registration {RegId}", registration.Id);
                }
            }
            await raceRepo.MarkEmailsSent(race.Id);
            _logger.LogInformation("Completed race processing finished. Total processed: {Count}", totalProcessed);
        }
    }
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Race Status Background Service is stopping");
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