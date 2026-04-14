namespace ObstaRaceChat.Application.Dto;

public sealed record SendMessageDto
{
        public int SenderId { get; init; }
        public string SenderName { get; init; } = string.Empty;
        public string SenderSurname { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty; 
}