using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using ObstaRaceChat.API.Hubs;
using ObstaRaceChat.Application.Helper;
using ObstaRaceChat.Application.Interfaces.Repository;
using ObstaRaceChat.Infrastructure.Configuration;
using ObstaRaceChat.Infrastructure.Repository;
using ObstaRaceChat.Application.Interfaces.Service;
using ObstaRaceChat.Application.Services;

var builder = WebApplication.CreateBuilder(args);
Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
//Bind mongodb settings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(nameof(MongoDbSettings)));

builder.Services.AddSingleton<IMongoClient>(
    sp => new MongoClient(builder.Configuration[$"{nameof(MongoDbSettings)}:ConnectionString"]));

// Auth & JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key missing");
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = key,
        ClockSkew = TimeSpan.Zero,
        ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha512 }
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Cookies["X-Access-Token"];
            if (string.IsNullOrEmpty(accessToken))
                accessToken = context.Request.Query["access_token"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                var cleanToken = accessToken.Trim().Trim('"');
                if (cleanToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    cleanToken = cleanToken.Substring(7).Trim();
                context.Token = cleanToken;
            }
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"--> JWT FAILED: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("--> JWT OK, user: " + context.Principal?.Identity?.Name);
            return Task.CompletedTask;
        }
    };
    
});

builder.Services.AddAuthorization();

builder.Services.AddOpenApi();
builder.Services.AddSignalR(opt =>
{
    opt.EnableDetailedErrors = true;
});
builder.Services.AddAutoMapper(cfg => {},typeof(MappingProfiles).Assembly);

//
builder.Services.AddScoped<IGlobalChatRepository, GlobalChatRepository>();
builder.Services.AddScoped<IGlobalChatService, GlobalChatService>();


var allowedOrigins = builder.Configuration.GetSection("FrontendSettings:AllowedOrigins").Get<string[]>();
if (allowedOrigins == null || allowedOrigins.Length == 0)
{
    throw new InvalidOperationException("You are not define AllowedOrigins!");
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<GlobalChatHub>("/hubs/global");

app.Run();