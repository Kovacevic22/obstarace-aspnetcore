using AutoMapper;
using ObstaRace.API.Dto;
using ObstaRace.API.Models;

namespace ObstaRace.API.Helper;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<User,UserDto>();
        CreateMap<Race,RaceDto>();
        CreateMap<Registration,RegistrationDto>();
        CreateMap<Obstacle,ObstacleDto>();
    }
}