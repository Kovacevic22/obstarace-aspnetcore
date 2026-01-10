using AutoMapper;
using ObstaRace.API.Dto;
using ObstaRace.API.Interfaces;
using ObstaRace.API.Interfaces.Services;

namespace ObstaRace.API.Services;

public class OrganiserService : IOrganiserService
{
    private readonly IMapper _mapper;
    private readonly IOrganiserRepository _organiserRepository;
    private readonly ILogger<OrganiserService> _logger;
    private readonly IUserRepository _userRepository;
    
    public OrganiserService(IMapper mapper, IOrganiserRepository organiserRepository, ILogger<OrganiserService> logger, IUserRepository userRepository)
    {
        _mapper = mapper;
        _organiserRepository = organiserRepository;
        _logger = logger;
        _userRepository = userRepository;
    }
    public async Task<ICollection<OrganiserDto>> GetPendingOrganisers()
    {
        _logger.LogInformation("Getting pending organisers");
        var organisers = await _organiserRepository.GetPendingOrganisers();
        return _mapper.Map<List<OrganiserDto>>(organisers);
    }

    public async Task<bool> VerifyOrganiser(int userId)
    {
        _logger.LogInformation("Verify organiser with userId {id}", userId);
        if (!await _userRepository.UserExists(userId))
        {
            _logger.LogError("User with id {id} don't exist", userId);
            throw new ArgumentException("User does not exist");
        }
        return await _organiserRepository.VerifyOrganiser(userId);
    }
}