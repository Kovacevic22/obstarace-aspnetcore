using FluentValidation;
using ObstaRace.Application.Dto;

namespace ObstaRace.Application.Validators;

public class OrganiserDtoValidator : AbstractValidator<OrganiserDto>
{
    public OrganiserDtoValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.OrganisationName).NotEmpty();
        RuleFor(x => x.Status).IsInEnum();
    }
}

public class RegisterOrganiserDtoValidator : AbstractValidator<RegisterOrganiserDto>
{
    public RegisterOrganiserDtoValidator()
    {
        RuleFor(x => x.OrganisationName)
            .NotEmpty().WithMessage("Organisation name is required.");
    }
}