using System.ComponentModel.DataAnnotations;

namespace ObstaRace.Application.Dto;

public class ParticipantDto
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string EmergencyContact { get; set; }
    public ParticipantActivityDto? Activity { get; set; }
}

public class RegisterParticipantDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
    [Required]
    [Phone]
    public string EmergencyContact { get; set; }
}

public class ParticipantActivityDto
{
    public int TotalRaces { get; set; }
    public int FinishedRaces { get; set; }
}
public class UpdateParticipantDto
{
    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
    [Required]
    [Phone]
    public string EmergencyContact { get; set; }
}