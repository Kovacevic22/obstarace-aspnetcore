using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Interfaces.Repositories;

public interface IOrganiserRepository
{
    Task<ICollection<Organiser>> GetPendingOrganisers();
    Task<bool> VerifyOrganiser(int userId);
    Task<bool> RejectOrganiser(int userId);
}