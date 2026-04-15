using AutoMapper;
using Microsoft.Extensions.Logging;
using ObstaRaceChat.Application.Dto;
using ObstaRaceChat.Application.Interfaces.Repository;
using ObstaRaceChat.Application.Interfaces.Service;
using ObstaRaceChat.Domain.Models;

namespace ObstaRaceChat.Application.Services;

public class GlobalChatService:IGlobalChatService
{
    private readonly ILogger<GlobalChatService> _logger;
    private readonly IMapper _mapper;
    private readonly IGlobalChatRepository _globalChatRepository;
    public GlobalChatService(IMapper mapper, IGlobalChatRepository globalChatRepository, ILogger<GlobalChatService> logger)
    {
        _logger = logger;
        _mapper = mapper;
        _globalChatRepository = globalChatRepository;
    }

    public async Task<ICollection<ReceiveMessageDto>> GetGlobalMessages()
    {
        _logger.LogInformation("Getting all global messages from database");
        var result = await _globalChatRepository.GetGlobalMessages();
        return _mapper.Map<ICollection<ReceiveMessageDto>>(result);
    }

    public async Task<ReceiveMessageDto> AddGlobalMessage(SendMessageDto content)
    {
        _logger.LogInformation("Adding new global message from user {SenderId}", content.SenderId);
        
        var message = _mapper.Map<GlobalMessage>(content);
        
        var receiveMessage =  await  _globalChatRepository.AddGlobalMessage(message);
        
        return _mapper.Map<ReceiveMessageDto>(receiveMessage);
    }

    public async Task<bool> DeleteGlobalMessage(string messageId)
    {
        _logger.LogInformation("Deleting global message with {MessageId} id", messageId);
        return await  _globalChatRepository.DeleteGlobalMessage(messageId);
    }
}