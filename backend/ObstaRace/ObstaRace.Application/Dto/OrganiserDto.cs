

using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Dto;

public sealed record OrganiserDto
{
    public int UserId { get; init; }
    public string UserName { get; init; } = null!;
    public string UserSurname { get; init; } = null!;
    public string UserEmail { get; init; } = null!;
    public string OrganisationName { get; init; } = null!;
    public string? Description { get; init; } 
    public OrganiserStatus Status { get; init; }
}
public sealed record RegisterOrganiserDto
{
    public string OrganisationName { get; init; } = null!;
    public string? Description { get; init; }
}