using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ObstaRace.Application.Interfaces.Repositories;
using ObstaRace.Application.Interfaces.Services;
using ObstaRace.Infrastructure.Configuration;
using ObstaRace.Infrastructure.Data;
using ObstaRace.Infrastructure.Repository;
using ObstaRace.Infrastructure.Service;

namespace ObstaRace.Infrastructure.Extensions;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Settings
        services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
        services.Configure<AwsSettings>(configuration.GetSection(nameof(AwsSettings)));
        services.Configure<ReminderSettings>(configuration.GetSection(nameof(ReminderSettings)));

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRaceRepository, RaceRepository>();
        services.AddScoped<IRegistrationRepository, RegistrationRepository>();
        services.AddScoped<IObstacleRepository, ObstacleRepository>();
        services.AddScoped<IOrganiserRepository, OrganiserRepository>();
        services.AddScoped<IParticipantRepository, ParticipantRepository>();

        // Services
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IFileService, FileService>();

        // Background Services
        services.AddHostedService<RaceReminderBgService>();
        services.AddHostedService<RaceStatusBgService>();

        // Redis
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "ObstaRace_";
        });

        // AWS S3
        var awsSettings = configuration.GetSection("AwsSettings").Get<AwsSettings>();
        services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client(
            awsSettings!.AccessKey,
            awsSettings!.SecretKey,
            Amazon.RegionEndpoint.GetBySystemName(awsSettings!.Region)
        ));

        return services;
    }
}