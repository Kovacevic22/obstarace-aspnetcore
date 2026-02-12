using AutoMapper;
using ObstaRace.Application.Dto;
using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Helper;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<User,UserDto>()
            .ForMember(dest => dest.Organiser, opt => opt.MapFrom(src => src.Organiser))
            .ForMember(dest => dest.Participant, opt => opt.MapFrom(src => src.Participant));
        CreateMap<UserDto,User>();
        CreateMap<Participant, ParticipantDto>()
            .ForMember(dest => dest.Activity, opt => opt.Ignore());
        CreateMap<UpdateParticipantDto, Participant>();
        CreateMap<RegisterParticipantDto, Participant>();
        
        CreateMap<Race,RaceDto>()
            .ForMember(dest => dest.Obstacles, opt => 
                opt.MapFrom(src => src.RaceObstacles.Select(ro => ro.Obstacle)))
            .ForMember(dest => dest.ObstacleIds, opt => 
                opt.MapFrom(src => src.RaceObstacles.Select(ro => ro.ObstacleId)));;
        CreateMap<RaceDto,Race>();
        CreateMap<CreateRaceDto, Race>();  
        CreateMap<UpdateRaceDto, Race>().ForMember(o => o.RaceObstacles, opt => opt.Ignore());;
        
        CreateMap<Registration,RegistrationDto>()
            .ForMember(dest => dest.Race, opt => opt.MapFrom(src => src.Race));
        CreateMap<RegistrationDto,Registration>();
        CreateMap<UpdateRegistrationDto, Registration>();
        
        CreateMap<Obstacle,ObstacleDto>();
        CreateMap<ObstacleDto,Obstacle>();
        
        CreateMap<CreateObstacleDto, Obstacle>();  
        CreateMap<UpdateObstacleDto, Obstacle>();

        CreateMap<Organiser, OrganiserDto>()
            .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email));
        CreateMap<OrganiserDto, Organiser>();
        CreateMap<RegisterOrganiserDto, Organiser>();
        
        CreateMap<RegisterDto, User>()
            .ForMember(dest => dest.Organiser, opt => opt.MapFrom(src => src.Organiser))
            .ForMember(dest => dest.Participant, opt => opt.MapFrom(src => src.Participant));
    }
}