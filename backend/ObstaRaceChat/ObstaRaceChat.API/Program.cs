using ObstaRaceChat.Application.Helper;
using ObstaRaceChat.Application.Interfaces.Repository;
using ObstaRaceChat.Infrastructure.Persistence;
using ObstaRaceChat.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSignalR();
builder.Services.AddAutoMapper(cfg => {},typeof(MappingProfiles).Assembly);

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<IChatRepository, ChatRepository>();

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.Run();