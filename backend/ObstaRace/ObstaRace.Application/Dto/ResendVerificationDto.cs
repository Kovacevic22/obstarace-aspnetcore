namespace ObstaRace.Application.Dto;

public sealed record ResendVerificationDto
{
        public string Email { get; init; } = null!;
}