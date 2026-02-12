

using ObstaRace.Application.Dto;
using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Interfaces.Repositories;

public interface IParticipantRepository
{
    Task<Participant?> GetParticipant(int userId);
    Task<ParticipantActivityDto> GetParticipantActivity(int userId);
    Task<bool> UpdateParticipant(Participant participant);
}