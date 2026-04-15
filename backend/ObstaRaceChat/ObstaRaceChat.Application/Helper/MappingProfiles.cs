using AutoMapper;
using ObstaRaceChat.Application.Dto;
using ObstaRaceChat.Domain;
using ObstaRaceChat.Domain.Models;

namespace ObstaRaceChat.Application.Helper;

public class MappingProfiles:Profile
{
    public MappingProfiles()
    {
        CreateMap<SendMessageDto, GlobalMessage>();

        CreateMap<GlobalMessage, ReceiveMessageDto>();
    }      
}