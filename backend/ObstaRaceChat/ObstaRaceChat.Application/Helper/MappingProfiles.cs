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

        CreateMap<GlobalMessage, ReceiveMessageDto>()
            .ForMember(dest => dest.SenderFullName, 
                opt => opt.MapFrom(src=>$"{src.SenderName} {src.SenderSurname}")); 
    }      
}