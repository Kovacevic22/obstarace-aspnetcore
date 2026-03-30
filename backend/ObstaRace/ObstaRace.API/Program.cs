using System.Text;
using Amazon.S3;
using AspNetCoreRateLimit;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ObstaRace.API.Middleware;
using ObstaRace.Application.Helper;
using ObstaRace.Application.Interfaces.Repositories;
using ObstaRace.Application.Interfaces.Services;
using ObstaRace.Application.Interfaces.Services.Auth;
using ObstaRace.Application.Services;
using ObstaRace.Application.Services.Auth;
using ObstaRace.Application.Validators;
using ObstaRace.Infrastructure.Configuration;
using ObstaRace.Infrastructure.Data;
using ObstaRace.Infrastructure.Repository;
using ObstaRace.Infrastructure.Seeders;
using ObstaRace.Infrastructure.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssembly(typeof(RaceDtoValidator).Assembly);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "ObstaRace_";
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
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
builder.Services.AddAutoMapper(cfg => {}, typeof(MappingProfiles).Assembly);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRaceRepository, RaceRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();
builder.Services.AddScoped<IObstacleRepository, ObstacleRepository>();
builder.Services.AddScoped<IObstacleService, ObstacleService>();
builder.Services.AddScoped<IRaceService, RaceService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrganiserRepository, OrganiserRepository>();
builder.Services.AddScoped<IOrganiserService, OrganiserService>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AwsSettings"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<ReminderSettings>(builder.Configuration.GetSection("ReminderSettings"));

var awsSettings = builder.Configuration.GetSection("AwsSettings").Get<AwsSettings>();
builder.Services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client(
    awsSettings!.AccessKey,
    awsSettings!.SecretKey,
    Amazon.RegionEndpoint.GetBySystemName(awsSettings!.Region)
));

builder.Services.AddHostedService<RaceReminderBgService>();
builder.Services.AddHostedService<RaceStatusBgService>();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
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
app.UseExceptionHandler();
app.UseIpRateLimiting();
app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    if (response.HasStarted) return;
    if (response.StatusCode == 403)
    {
        response.ContentType = "application/json";
        await response.WriteAsJsonAsync(new 
        { 
            status = 403, 
            error = "Forbidden",
            message = "You do not have permission to access this resource."
        });
    }
    else if (response.StatusCode == 401)
    {
        response.ContentType = "application/json";
        await response.WriteAsJsonAsync(new 
        { 
            status = 401, 
            error = "Unauthorized",
            message = "Authentication is required."
        });
    }
});
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<BanCheckMiddleware>();
app.MapControllers();
app.Run();