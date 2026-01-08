using AutoMapper;
using ObstaRace.API.Dto;
using ObstaRace.API.Models;

namespace ObstaRace.API.Helper;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<User,UserDto>();
        CreateMap<UserDto,User>();
        
        CreateMap<Race,RaceDto>();
        CreateMap<RaceDto,Race>();
        CreateMap<CreateRaceDto, Race>();  
        CreateMap<UpdateRaceDto, Race>().ForMember(o => o.RaceObstacles, opt => opt.Ignore());;
        
        CreateMap<Registration,RegistrationDto>();
        CreateMap<RegistrationDto,Registration>();
        CreateMap<UpdateRegistrationDto, Registration>();
        
        CreateMap<Obstacle,ObstacleDto>();
        CreateMap<ObstacleDto,Obstacle>();
        CreateMap<CreateObstacleDto, Obstacle>();  
        CreateMap<UpdateObstacleDto, Obstacle>();
    }
}