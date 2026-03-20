namespace ObstaRaceChat.Domain;

public sealed record GlobalMessage
{
    public string? Id { get; init; }
    public int SenderId { get; init; } 
    public string SenderName { get; init; } = string.Empty;
    public string SenderSurname { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty; 
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}