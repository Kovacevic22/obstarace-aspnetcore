using FluentValidation;
using ObstaRace.Application.Dto;

namespace ObstaRace.Application.Validators;

public class ObstacleDtoValidator : AbstractValidator<ObstacleDto>
{
    public ObstacleDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.Difficulty)
            .IsInEnum();
    }
}

public class CreateObstacleDtoValidator : AbstractValidator<CreateObstacleDto>
{
    public CreateObstacleDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name is too long");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description is too long");

        RuleFor(x => x.Difficulty)
            .IsInEnum().WithMessage("Difficulty is required");
    }
}

public class UpdateObstacleDtoValidator : AbstractValidator<UpdateObstacleDto>
{
    public UpdateObstacleDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.Difficulty)
            .IsInEnum();
    }
}