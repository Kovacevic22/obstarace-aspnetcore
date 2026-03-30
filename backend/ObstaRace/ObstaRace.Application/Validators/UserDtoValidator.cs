using FluentValidation;
using ObstaRace.Application.Dto;

namespace ObstaRace.Application.Validators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.Role).IsInEnum();
    }
}

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"^[0-9+\s-]*$").WithMessage("Invalid phone format.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
        RuleFor(x => x)
            .Must(x => !(x.Organiser != null && x.Participant != null))
            .WithMessage("Cannot register as both organiser and participant.");

        RuleFor(x => x)
            .Must(x => x.Organiser != null || x.Participant != null)
            .WithMessage("Must provide either participant or organiser data.");
        RuleFor(x => x.Participant)
            .SetValidator(new RegisterParticipantDtoValidator()!)
            .When(x => x.Participant != null);

        RuleFor(x => x.Organiser)
            .SetValidator(new RegisterOrganiserDtoValidator()!)
            .When(x => x.Organiser != null);
    }
}

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}