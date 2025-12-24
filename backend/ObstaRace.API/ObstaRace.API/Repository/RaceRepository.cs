using ObstaRace.API.Data;
using ObstaRace.API.Interfaces;
using ObstaRace.API.Models;

namespace ObstaRace.API.Repository;

public class RaceRepository : IRaceRepository
{
    private readonly DataContext _context;
    public RaceRepository(DataContext context)
    {
        _context = context;
    }
    public ICollection<Race> GetAllRaces()
    {
        return _context.Races.OrderBy(r => r.Id).ToList();
    }

    public Race GetRace(int id)
    {
        return _context.Races.Where(r => r.Id == id).FirstOrDefault();
    }

    public bool RaceExists(int id)
    {
        return _context.Races.Any(r => r.Id == id);
    }
}