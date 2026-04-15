using System.Collections.Generic;
using System.Threading.Tasks;
using ObstaRaceChat.Domain.Models;

namespace ObstaRaceChat.Application.Interfaces.Repository;

public interface IGlobalChatRepository
{
    Task<ICollection<GlobalMessage>> GetGlobalMessages();
    Task<GlobalMessage>  AddGlobalMessage(GlobalMessage message);
    Task<bool> DeleteGlobalMessage(string messageId);
    Task<GlobalMessage> GetMessageById(string messageId);
}