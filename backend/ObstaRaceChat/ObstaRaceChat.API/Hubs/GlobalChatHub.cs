using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        await base.OnConnectedAsync();
        var history = await _globalChatService.GetGlobalMessages();
        await Clients.Caller.LoadHistory(history);
    }
    [Authorize]
    public async Task SendGlobalMessage(string message)
    {
        var userIdClaim = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = Context.User?.Identity?.Name;
        if (userIdClaim == null || userName == null)
        {
            Console.WriteLine("--> ERROR: User claims is NULL!");
            return;
        }
        int userId = int.Parse(userIdClaim);
        var content = new SendMessageDto()
        {
            SenderId = userId,
            Content = message,
            SenderFullName = userName,
        };
        var saved = await _globalChatService.AddGlobalMessage(content);
        await Clients.All.ReceiveGlobalMessage(saved);
    }
    [Authorize]
    public async Task DeleteGlobalMessage(string messageId)
    {
        var userIdClaim = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userIdClaim==null)return;
        int userId = int.Parse(userIdClaim);
        var success =  await _globalChatService.DeleteGlobalMessage(messageId, userId);
        if (success) await Clients.All.MessageDeleted(messageId);
    }
}