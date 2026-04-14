using ObstaRaceChat.Application.Dto;
using ObstaRaceChat.Application.Interfaces.Service;
using ObstaRaceChat.Application.Services;

namespace ObstaRaceChat.API.Extensions;

public static class ChatHubExtension
{
    public static RouteGroupBuilder MapChatEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/chat");
        group.MapGet("/", GetMessage);
        group.MapPost("/send", SendMessage);
        return group;
    }

    private static async Task<IResult> GetMessage(IGlobalChatService globalChatService)
    {
        var result = await globalChatService.GetGlobalMessages();
        return Results.Ok(result);
    }

    private static async Task<IResult> SendMessage(SendMessageDto content,IGlobalChatService globalChatService)
    {
        var result = await globalChatService.AddGlobalMessage(content);
        return result ? Results.Ok(new { success = true }) : Results.BadRequest();    }
}