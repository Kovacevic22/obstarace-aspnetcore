using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using ObstaRaceChat.Application.Dto;
using ObstaRaceChat.Application.Interfaces.Service;

namespace ObstaRaceChat.API.Hubs;

public sealed class GlobalChatHub : Hub<IGlobalChatClient>
{
    private readonly IGlobalChatService _globalChatService;
    public GlobalChatHub(IGlobalChatService globalChatService)
    {
        _globalChatService = globalChatService;
    }

    public override async Task OnConnectedAsync()
    {
        var history = await _globalChatService.GetGlobalMessages();
        await Clients.Caller.LoadHistory(history);
        await base.OnConnectedAsync();
    }

    public async Task SendGlobalMessage(SendMessageDto messageDto)
    {
        var saved = await _globalChatService.AddGlobalMessage(messageDto);
        await Clients.All.ReceiveGlobalMessage(saved);
    }

    public async Task DeleteGlobalMessage(string messageId)
    {
        var success =  await _globalChatService.DeleteGlobalMessage(messageId);
        if (success) await Clients.All.MessageDeleted(messageId);
    }
}