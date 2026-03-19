using System.ComponentModel.DataAnnotations;

namespace ObstaRace.Application.Dto;

public sealed record ResendVerificationDto
{
        [Required][EmailAddress] public string Email { get; init; } = null!;
}