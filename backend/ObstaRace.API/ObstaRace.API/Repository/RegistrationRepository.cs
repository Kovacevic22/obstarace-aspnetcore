using ObstaRace.API.Data;
using ObstaRace.API.Models;

namespace ObstaRace.API.Repository;

public class RegistrationRepository
{
    private readonly DataContext _context;
    public RegistrationRepository(DataContext context)
    {
        _context = context;
    }

    public ICollection<Registration> GetAllRegistrations()
    {
        return _context.Registrations.OrderBy(r => r.Id).ToList();
    }

    public Registration GetRegistration(int id)
    {
        return _context.Registrations.Where(r => r.Id == id).FirstOrDefault();
    }
    public bool RegistrationExists(int id)
    {
        return _context.Registrations.Any(r => r.Id == id);
    }
}