using Microsoft.EntityFrameworkCore;
using ObstaRace.Application.Interfaces.Repositories;
using ObstaRace.Domain.Models;
using ObstaRace.Infrastructure.Data;

namespace ObstaRace.Infrastructure.Repository;

public class RegistrationRepository : IRegistrationRepository
{
    private DataContext _context;
    public RegistrationRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Registration>> GetAllRegistrations(int? userId, int? page, int? pageSize)
    {
        var query = userId!=null? _context.Registrations
                .AsNoTracking()
            .Include(r => r.Race)
            .Include(r => r.Participant)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r=>r.CreatedAt):
            _context.Registrations
                .AsNoTracking()
                .Include(r => r.Race)
                .Include(r => r.Participant)
                .OrderByDescending(r=>r.CreatedAt);
        if (page.HasValue && pageSize.HasValue)
        {
            query = (IOrderedQueryable<Registration>)query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }
        return await query.ToListAsync();
    }

    public async Task<ICollection<Registration>> GetParticipantsForRace(int organiserId, int? raceId, int? page, int? pageSize)
    {
        var query = _context.Registrations
            .AsNoTracking()
            .Include(r => r.Race)
            .ThenInclude(ra => ra.RaceObstacles)
            .ThenInclude(ro => ro.Obstacle)
            .Include(r => r.User)
            .Include(r => r.Participant)
            .Where(r => r.Race.CreatedById == organiserId && r.Status == RegistrationStatus.Pending)
            .OrderByDescending(r => r.CreatedAt); 
        
        if (raceId.HasValue && raceId > 0)
        {
            query = (IOrderedQueryable<Registration>)query.Where(r => r.RaceId == raceId.Value);
        }

        if (page.HasValue && pageSize.HasValue)
        {
            query = (IOrderedQueryable<Registration>)query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }
        return await query.ToListAsync();
    }

    public async Task<Registration?> GetRegistration(int id)
    {
        return await _context.Registrations
            .Include(r => r.User)
            .Include(r => r.Participant)
            .Include(r => r.Race)
            .ThenInclude(ra => ra.RaceObstacles)
            .ThenInclude(ro => ro.Obstacle)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
    public async Task<bool> RegistrationExists(int id)
    {
        return await _context.Registrations.AnyAsync(r => r.Id == id);
    }
    //CRUD
    public async Task<bool> CreateRegistration(Registration registration)
    {
        _context.Registrations.Add(registration);
        return await SaveChanges();
    }
    public async Task<bool> UpdateRegistration(Registration registration)
    {
        _context.Registrations.Update(registration);
        return await SaveChanges();
    }
    public async Task<bool> DeleteRegistration(int registrationId)
    {
        var registration = await _context.Registrations.FindAsync(registrationId);
        if (registration == null) return false;
    
        _context.Registrations.Remove(registration);
        return await SaveChanges();
    }
    public async IAsyncEnumerable<Registration> StreamRegistrationsForCompletedRace(int raceId)
    {
        await foreach (var registration in _context.Registrations
                           .Include(r => r.User)
                           .ThenInclude(u => u.Participant)
                           .Where(r => 
                               r.RaceId == raceId && 
                               r.Status == RegistrationStatus.Confirmed)
                           .AsAsyncEnumerable())
        {
            yield return registration;
        }
    }
    public async IAsyncEnumerable<Registration> GetRegistrationsForReminderAsync(DateTime targetDate)
    {
        var start = targetDate.Date;
        var end = start.AddDays(1);
        await foreach (var registration in _context.Registrations
                           .Include(r => r.Race)
                           .Include(r => r.User)
                           .ThenInclude(u => u.Participant)
                           .Where(r => 
                               r.Status == RegistrationStatus.Confirmed &&  
                               !r.ReminderSent &&                           
                               r.Race.Date >= start 
                               && r.Race.Date < end
                           )
                           .AsAsyncEnumerable())
        {
            yield return registration;
        }
    }

    //ADDITIONAL METHODS
    public async Task<int> GetNextBibNumber()
    {
        var max = await _context.Registrations
            .MaxAsync(r => (int?)Convert.ToInt32(r.BibNumber)) ?? 0;
        return max + 1;
    }
    public async Task<bool> SaveChanges()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0;
    }

    public async Task<Registration?> GetRegistrationByUserId(int userId)
    {
        return await _context.Registrations.FirstOrDefaultAsync(r => r.UserId == userId); 
    }

    public async Task<List<Registration>?> GetRegistrationsByRaceId(int raceId)
    {
        return await _context.Registrations.Where(r => r.RaceId == raceId).ToListAsync();
    }

    public async Task<bool> UserRegistered(int userId, int raceId)
    {
        return await _context.Registrations.AnyAsync(r => r.UserId == userId && r.RaceId == raceId);
    }

    public async Task<int> CountRegistrations(int raceId)
    {
        return await _context.Registrations
            .CountAsync(r => r.RaceId == raceId && r.Status != RegistrationStatus.Cancelled && r.Status != RegistrationStatus.Pending);
    }
}