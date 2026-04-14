using ObstaRaceChat.Application.Dto;
using ObstaRaceChat.Domain.Models;

namespace ObstaRaceChat.Application.Interfaces.Service;

public interface IGlobalChatService
{
    Task<ICollection<ReceiveMessageDto>> GetGlobalMessages();
    Task<bool> AddGlobalMessage(SendMessageDto message);
    Task<bool> DeleteGlobalMessage(string messageId);
}