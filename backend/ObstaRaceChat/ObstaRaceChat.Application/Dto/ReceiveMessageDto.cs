namespace ObstaRaceChat.Application;

public sealed record ReceiveMessageDto
{
    public string Id { get; init; } = null!; 
    public int SenderId { get; init; }
    public string SenderFullName { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}