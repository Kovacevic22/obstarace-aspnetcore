using System.Text;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ObstaRace.Application.Interfaces.Repositories;
using ObstaRace.Application.Interfaces.Services;
using ObstaRace.Application.Services;
using ObstaRace.Infrastructure.Configuration;
using ObstaRace.Infrastructure.Data;
using ObstaRace.Infrastructure.Repository;
using ObstaRace.Infrastructure.Seeders;
using ObstaRace.Infrastructure.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "default_key_at_least_32_chars_long")),
        ClockSkew = TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["X-Access-Token"];
            return Task.CompletedTask;
        }
    };
});
var allowedOrigins = builder.Configuration.GetSection("FrontendSettings:AllowedOrigins").Get<string[]>();
if (allowedOrigins == null || allowedOrigins.Length == 0)
{
    throw new InvalidOperationException("You are not define AllowedOrigins!");
}
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(typeof(ObstaRace.Application.Helper.MappingProfiles));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRaceRepository, RaceRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IObstacleRepository, ObstacleRepository>();
builder.Services.AddScoped<IObstacleService, ObstacleService>();
builder.Services.AddScoped<IRaceService, RaceService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrganiserRepository, OrganiserRepository>();
builder.Services.AddScoped<IOrganiserService, OrganiserService>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<ReminderSettings>(
    builder.Configuration.GetSection("ReminderSettings")
);

builder.Services.AddHostedService<RaceReminderBgService>();
builder.Services.AddHostedService<RaceStatusBgService>();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("AdditionalConnection"));
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await DataSeeder.SeedAsync(context);
}
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseIpRateLimiting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();