using ObstaRaceChat.Application.Dto;
using ObstaRaceChat.Domain.Models;

namespace ObstaRaceChat.Application.Interfaces.Service;

public interface IGlobalChatClient
{
    Task ReceiveGlobalMessage(ReceiveMessageDto message);
    Task LoadHistory(ICollection<ReceiveMessageDto> message);
    Task MessageDeleted(string messageId);
}