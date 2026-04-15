using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ObstaRaceChat.Application.Dto;
using ObstaRaceChat.Application.Interfaces.Repository;
using ObstaRaceChat.Domain.Models;
using ObstaRaceChat.Infrastructure.Configuration;

namespace ObstaRaceChat.Infrastructure.Repository;

public class GlobalChatRepository : IGlobalChatRepository
{
    private readonly IMongoCollection<GlobalMessage> _messages;
    public GlobalChatRepository(IMongoClient client,IOptions<MongoDbSettings> settings)
    {
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _messages = database.GetCollection<GlobalMessage>("global_messages");
    }
    public async Task<ICollection<GlobalMessage>> GetGlobalMessages()
    {
        return await _messages.Find(_ => true)
            .SortByDescending(m => m.Timestamp)
            .Limit(50)
            .ToListAsync();
    }

    public async Task<GlobalMessage> AddGlobalMessage(GlobalMessage message)
    {
        await _messages.InsertOneAsync(message);
        return message;
    }

    public async Task<bool> DeleteGlobalMessage(string messageId)
    {
        var result = await _messages.DeleteOneAsync(m => m.Id == messageId);
        return result.DeletedCount > 0;
    }
}