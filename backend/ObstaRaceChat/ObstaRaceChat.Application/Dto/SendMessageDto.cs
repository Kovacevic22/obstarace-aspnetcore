namespace ObstaRaceChat.Application.Dto;

public sealed record SendMessageDto
{
        public int SenderId { get; init; }
        public string SenderFullName { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty; 
}