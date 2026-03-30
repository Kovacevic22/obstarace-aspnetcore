using FluentValidation;
using ObstaRace.Application.Dto;

namespace ObstaRace.Application.Validators;

public class ParticipantDtoValidator : AbstractValidator<ParticipantDto>
{
    public ParticipantDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Surname).NotEmpty();
        RuleFor(x => x.DateOfBirth).NotEmpty();
    }
}

public class RegisterParticipantDtoValidator : AbstractValidator<RegisterParticipantDto>
{
    public RegisterParticipantDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Surname).NotEmpty();
        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .Must(BeAtLeast18)
            .WithMessage("Must be at least 18 years old.");;
        RuleFor(x => x.EmergencyContact)
            .NotEmpty()
            .Matches(@"^[0-9+\s-]*$").WithMessage("Invalid phone format.");
    }
    private bool BeAtLeast18(DateOnly dateOfBirth)
    {
        return dateOfBirth <= DateOnly.FromDateTime(DateTime.Today).AddYears(-18);
    }
}

public class UpdateParticipantDtoValidator : AbstractValidator<UpdateParticipantDto>
{
    public UpdateParticipantDtoValidator()
    {
        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .Must(BeAtLeast18)
            .WithMessage("Must be at least 18 years old.");;
        RuleFor(x => x.EmergencyContact)
            .NotEmpty()
            .Matches(@"^[0-9+\s-]*$").WithMessage("Invalid phone format.");
    }
    private bool BeAtLeast18(DateOnly dateOfBirth)
    {
        return dateOfBirth <= DateOnly.FromDateTime(DateTime.Today).AddYears(-18);
    }
}
