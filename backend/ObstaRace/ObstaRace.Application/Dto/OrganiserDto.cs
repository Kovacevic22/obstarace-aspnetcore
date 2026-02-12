

using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Dto;

public class OrganiserDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } 
    public string UserSurname { get; set; } 
    public string UserEmail { get; set; } 
    public string OrganisationName { get; set; }
    public string Description { get; set; } 
    public OrganiserStatus Status { get; set; }
}
public class RegisterOrganiserDto
{
    public string OrganisationName { get; set; }
    public string Description { get; set; }
}