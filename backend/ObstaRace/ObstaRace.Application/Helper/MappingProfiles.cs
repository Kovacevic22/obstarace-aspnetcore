using AutoMapper;
using ObstaRace.Application.Dto;
using ObstaRace.Domain.Models;

namespace ObstaRace.Application.Helper;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<User, UserDto>();
        CreateMap<Organiser, OrganiserDto>();
        
        CreateMap<Participant, ParticipantDto>()
            .ForMember(dest => dest.Activity, opt => opt.Ignore());
        
        CreateMap<RegisterDto, User>();
        
        CreateMap<RegisterParticipantDto, Participant>();
        CreateMap<RegisterOrganiserDto, Organiser>();
        
        CreateMap<Race, RaceDto>()
            .ForMember(dest => dest.Obstacles, opt => 
                opt.MapFrom(src => src.RaceObstacles.Select(ro => ro.Obstacle)))
            .ForMember(dest => dest.ObstacleIds, opt => 
                opt.MapFrom(src => src.RaceObstacles.Select(ro => ro.ObstacleId)));
        
        CreateMap<Obstacle, ObstacleDto>().ReverseMap();
        
        CreateMap<CreateObstacleDto, Obstacle>();
        CreateMap<UpdateObstacleDto, Obstacle>();

        CreateMap<CreateRaceDto, Race>();
        CreateMap<UpdateRaceDto, Race>()
            .ForMember(dest => dest.RaceObstacles, opt => opt.Ignore());
        
        CreateMap<Registration, RegistrationDto>();
        
        CreateMap<UpdateRegistrationDto, Registration>().ReverseMap();
    }
}