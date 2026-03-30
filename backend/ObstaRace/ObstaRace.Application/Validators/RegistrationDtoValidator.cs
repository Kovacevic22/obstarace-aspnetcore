using FluentValidation;
using ObstaRace.Application.Dto;

namespace ObstaRace.Application.Validators;

public class RegistrationDtoValidator : AbstractValidator<RegistrationDto>
{
    public RegistrationDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.")
            .GreaterThan(0).WithMessage("Invalid User ID.");

        RuleFor(x => x.RaceId)
            .NotEmpty().WithMessage("Race ID is required.")
            .GreaterThan(0).WithMessage("Invalid Race ID.");

        RuleFor(x => x.BibNumber)
            .NotEmpty().WithMessage("Bib number is required.")
            .MaximumLength(10).WithMessage("Bib number cannot exceed 10 characters.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid registration status.");
    }
}

public class CreateRegistrationDtoValidator : AbstractValidator<CreateRegistrationDto>
{
    public CreateRegistrationDtoValidator()
    {
        RuleFor(x => x.RaceId)
            .GreaterThan(0).WithMessage("Valid Race ID is required.");

        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("Valid User ID is required.");
    }
}

public class UpdateRegistrationDtoValidator : AbstractValidator<UpdateRegistrationDto>
{
    public UpdateRegistrationDtoValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("A valid status must be provided.");

        RuleFor(x => x.BibNumber)
            .MaximumLength(10)
            .When(x => !string.IsNullOrEmpty(x.BibNumber));
            
        RuleFor(x => x.UserId)
            .GreaterThan(0);
            
        RuleFor(x => x.RaceId)
            .GreaterThan(0);
    }
}