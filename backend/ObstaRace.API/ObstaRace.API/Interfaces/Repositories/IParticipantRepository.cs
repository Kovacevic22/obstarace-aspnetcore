using ObstaRace.API.Dto;
using ObstaRace.API.Models;

namespace ObstaRace.API.Interfaces;

public interface IParticipantRepository
{
    Task<Participant?> GetParticipant(int userId);
    Task<ParticipantActivityDto> GetParticipantActivity(int userId);
    Task<bool> UpdateParticipant(Participant participant);
}