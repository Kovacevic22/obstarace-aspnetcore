using FluentValidation;
using ObstaRace.Application.Dto;

namespace ObstaRace.Application.Validators;

public class ResendVerificationDtoValidator:AbstractValidator<ResendVerificationDto>
{
    public ResendVerificationDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}