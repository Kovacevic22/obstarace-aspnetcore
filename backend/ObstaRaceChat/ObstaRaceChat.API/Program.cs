using MongoDB.Driver;
using ObstaRaceChat.API.Hubs;
using ObstaRaceChat.Application.Helper;
using ObstaRaceChat.Application.Interfaces.Repository;
using ObstaRaceChat.Infrastructure.Configuration;
using ObstaRaceChat.Infrastructure.Repository;
using ObstaRaceChat.Application.Interfaces.Service;
using ObstaRaceChat.Application.Services;


var builder = WebApplication.CreateBuilder(args);

//Bind mongodb settings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(nameof(MongoDbSettings)));

builder.Services.AddSingleton<IMongoClient>(
    sp => new MongoClient(builder.Configuration[$"{nameof(MongoDbSettings)}:ConnectionString"]));

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

app.UseHttpsRedirection();

app.MapHub<GlobalChatHub>("/hubs/global");

app.Run();