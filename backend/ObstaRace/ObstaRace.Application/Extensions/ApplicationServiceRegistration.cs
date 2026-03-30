using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ObstaRace.Application.Helper;
using ObstaRace.Application.Interfaces.Services;
using ObstaRace.Application.Interfaces.Services.Auth;
using ObstaRace.Application.Services;
using ObstaRace.Application.Services.Auth;
using ObstaRace.Application.Validators;

namespace ObstaRace.Application.Extensions;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => {}, typeof(MappingProfiles).Assembly);
        services.AddValidatorsFromAssembly(typeof(RaceDtoValidator).Assembly);

        services.AddScoped<IObstacleService, ObstacleService>();
        services.AddScoped<IRaceService, RaceService>();
        services.AddScoped<IRegistrationService, RegistrationService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOrganiserService, OrganiserService>();

        return services;
    }
}