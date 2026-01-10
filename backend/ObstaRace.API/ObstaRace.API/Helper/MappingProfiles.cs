using AutoMapper;
using ObstaRace.API.Dto;
using ObstaRace.API.Models;

namespace ObstaRace.API.Helper;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<User,UserDto>()
            .ForMember(dest => dest.Organiser, opt => opt.MapFrom(src => src.Organiser))
            .ForMember(ac => ac.Activity, 
                opt => opt.MapFrom(src => new UserActivityDto()
                {
                    TotalRaces = src.Registrations.Count,
                    FinishedRaces = src.Registrations.Count(r => r.Status == RegistrationStatus.Finished)
                }));
        CreateMap<UserDto,User>();
        CreateMap<UpdateUserDto, User>();
        
        CreateMap<Race,RaceDto>().ForMember(dest => dest.Obstacles, opt => opt.MapFrom(src => src.RaceObstacles.Select(ro => ro.Obstacle))).ForMember(dest => dest.ObstacleIds, opt => opt.MapFrom(src => src.RaceObstacles.Select(ro => ro.ObstacleId)));;
        CreateMap<RaceDto,Race>();
        CreateMap<CreateRaceDto, Race>();  
        CreateMap<UpdateRaceDto, Race>().ForMember(o => o.RaceObstacles, opt => opt.Ignore());;
        
        CreateMap<Registration,RegistrationDto>().ForMember(dest => dest.Race, opt => opt.MapFrom(src => src.Race));
        CreateMap<RegistrationDto,Registration>();
        CreateMap<UpdateRegistrationDto, Registration>();
        
        CreateMap<Obstacle,ObstacleDto>();
        CreateMap<ObstacleDto,Obstacle>();
        
        CreateMap<CreateObstacleDto, Obstacle>();  
        CreateMap<UpdateObstacleDto, Obstacle>();

        CreateMap<Organiser, OrganiserDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.UserSurname, opt => opt.MapFrom(src => src.User.Surname))
            .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User.Email));
        CreateMap<OrganiserDto, Organiser>();
        CreateMap<RegisterDto, User>()
            .ForMember(dest => dest.Organiser, opt => opt.MapFrom(src => src.Organiser));
    }
}