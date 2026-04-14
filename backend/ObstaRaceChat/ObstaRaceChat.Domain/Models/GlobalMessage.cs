using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ObstaRaceChat.Domain.Models;

public sealed record GlobalMessage
{
    [BsonId]
    [BsonElement("_id"),BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; init; }
    [BsonElement("senderId")]
    public int SenderId { get; init; } 
    [BsonElement("senderName")]
    public string SenderName { get; init; } = string.Empty;
    [BsonElement("senderSurname")]
    public string SenderSurname { get; init; } = string.Empty;
    [BsonElement("content")]
    public string Content { get; init; } = string.Empty; 
    [BsonElement("timestamp")]
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}