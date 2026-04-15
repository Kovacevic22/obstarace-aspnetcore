using ObstaRaceChat.Application.Dto;
using ObstaRaceChat.Domain;
using ObstaRaceChat.Domain.Models;

namespace ObstaRaceChat.Application.Interfaces.Repository;

public interface IGlobalChatRepository
{
    Task<ICollection<GlobalMessage>> GetGlobalMessages();
    Task<GlobalMessage>  AddGlobalMessage(GlobalMessage message);
    Task<bool> DeleteGlobalMessage(string messageId);
}