using ObstaRace.API.Dto;

namespace ObstaRace.API.Interfaces.Services;

public interface IOrganiserService
{
    Task<ICollection<OrganiserDto>> GetPendingOrganisers();
    Task<bool> VerifyOrganiser(int userId);
    Task<bool> RejectOrganiser(int userId);
}