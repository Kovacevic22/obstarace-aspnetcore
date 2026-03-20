namespace ObstaRaceChat.Application;

public sealed record SendMessageDto
{
    public string Content { get; init; } = string.Empty;
}