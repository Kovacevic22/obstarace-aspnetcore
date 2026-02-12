using Microsoft.EntityFrameworkCore;
using ObstaRace.Application.Interfaces.Repositories;
using ObstaRace.Domain.Models;
using ObstaRace.Infrastructure.Data;

namespace ObstaRace.Infrastructure.Repository;

public class OrganiserRepository :  IOrganiserRepository
{
    private readonly DataContext _context;
    public OrganiserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Organiser>> GetAllOrganisers()
    {
        return await _context.Organisers.OrderBy(o => o.UserId).ToListAsync();
    }

    public async Task<ICollection<Organiser>> GetPendingOrganisers()
    {
        return await _context.Organisers
            .Include(o => o.User)
            .Where(o => o.Status == OrganiserStatus.Pending)
            .ToListAsync();
    }

    public async Task<bool> VerifyOrganiser(int userId)
    {
        var organiser = await  _context.Organisers.FindAsync(userId);
        if (organiser == null) return false;
        organiser.Status = OrganiserStatus.Approved;
        return await SaveChanges();
    }
    public async Task<bool> RejectOrganiser(int userId)
    {
        var organiser = await _context.Organisers.FindAsync(userId);
        if (organiser == null) return false;
    
        organiser.Status = OrganiserStatus.Rejected;
        return await SaveChanges();
    }
    ///////////////////////////////////////
    public async Task<bool> SaveChanges()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0;
    }
}