

using ObstaRace.Application.Dto;
using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Interfaces.Repositories;

public interface IParticipantRepository
{
    Task<ParticipantActivityDto> GetParticipantActivity(int userId);
    Task<bool> UpdateParticipant(Participant participant);
    public Task<Dictionary<int, ParticipantActivityDto>> GetActivitiesForUsers(List<int> userIds);
}