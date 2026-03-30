namespace ObstaRaceChat.Infrastructure.Persistence;

public sealed record MongoDbSettings
{
    public string ConnectionString { get; init; } = null!;
    public string DatabaseName { get; init; } = null!;
}