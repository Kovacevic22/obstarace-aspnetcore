using Microsoft.EntityFrameworkCore;
using ObstaRace.Application.Dto;
using ObstaRace.Application.Interfaces.Repositories;
using ObstaRace.Domain.Models;
using ObstaRace.Infrastructure.Data;


namespace ObstaRace.Infrastructure.Repository;

public class ParticipantRepository : IParticipantRepository
{
    private readonly DataContext _context;
    
    public ParticipantRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Participant?> GetParticipant(int userId)
    {
        return await _context.Participants
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<ParticipantActivityDto> GetParticipantActivity(int userId)
    {
        var totalRaces = await _context.Registrations
            .CountAsync(r => r.UserId == userId);
        var finishedRaces = await _context.Registrations
            .CountAsync(r => r.UserId == userId && r.Status == RegistrationStatus.Finished);
            
        return new ParticipantActivityDto
        {
            TotalRaces = totalRaces,
            FinishedRaces = finishedRaces
        };
    }

    public async Task<bool> UpdateParticipant(Participant participant)
    {
        _context.Participants.Update(participant);
        var saved = await _context.SaveChangesAsync();
        return saved > 0;
    }
}