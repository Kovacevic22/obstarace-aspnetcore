using ObstaRace.API.Models;

namespace ObstaRace.API.Interfaces;

public interface IOrganiserRepository
{
    Task<ICollection<Organiser>> GetAllOrganisers();
    Task<ICollection<Organiser>> GetPendingOrganisers();
    Task<bool> VerifyOrganiser(int userId);
    Task<bool> RejectOrganiser(int userId);
}