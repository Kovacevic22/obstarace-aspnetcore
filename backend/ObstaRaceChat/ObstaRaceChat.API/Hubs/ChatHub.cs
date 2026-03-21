using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace ObstaRaceChat.API.Hubs;

public sealed class ChatHub : Hub
{
    private readonly IMapper _mapper;
    public ChatHub(IMapper mapper)
    {
        _mapper = mapper;
    }
}