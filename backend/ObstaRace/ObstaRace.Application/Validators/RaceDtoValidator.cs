using FluentValidation;
using ObstaRace.Application.Dto;

namespace ObstaRace.Application.Validators;

public class RaceDtoValidator : AbstractValidator<RaceDto>
{
    public RaceDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Race name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required.")
            .MaximumLength(100);
        
        RuleFor(x => x.Date)
            .Must(date => date > DateTime.UtcNow)
            .WithMessage("Race date must be in the future.");

        RuleFor(x => x.RegistrationDeadLine)
            .NotEmpty().WithMessage("Registration deadline is required.")
            .LessThanOrEqualTo(x => x.Date.AddDays(-7))
            .WithMessage("Registration deadline must be at least 7 days before the race date.");

        RuleFor(x => x.Distance)
            .InclusiveBetween(0.1, 1000.0).WithMessage("Distance must be between 0.1 and 1000 km.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(200);

        RuleFor(x => x.MaxParticipants)
            .InclusiveBetween(1, 10000).WithMessage("Max participants must be between 1 and 10000.");

        RuleFor(x => x.ImageUrl)
            .Must(uri => string.IsNullOrEmpty(uri) || Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("Image URL format is not valid.");
            
        RuleFor(x => x.Difficulty)
            .IsInEnum().WithMessage("Invalid difficulty level.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid race status.");
    }
}

public class CreateRaceDtoValidator : AbstractValidator<CreateRaceDto>
{
    public CreateRaceDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Race name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required.")
            .MaximumLength(100);

        RuleFor(x => x.Date)
            .Must(date => date > DateTime.UtcNow)
            .WithMessage("Race date must be in the future.");

        RuleFor(x => x.RegistrationDeadLine)
            .NotEmpty().WithMessage("Registration deadline is required.")
            .LessThanOrEqualTo(x => x.Date.AddDays(-7))
            .WithMessage("Registration deadline must be at least 7 days before the race date.");

        RuleFor(x => x.Distance)
            .InclusiveBetween(0.1, 1000.0).WithMessage("Distance must be between 0.1 and 1000 km.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(200);

        RuleFor(x => x.MaxParticipants)
            .InclusiveBetween(1, 10000).WithMessage("Max participants must be between 1 and 10000.");

        RuleFor(x => x.ImageUrl)
            .Must(uri => string.IsNullOrEmpty(uri) || Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("Image URL format is not valid.");

        RuleFor(x => x.Difficulty)
            .IsInEnum().WithMessage("Invalid difficulty level.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid race status.");
            
        RuleFor(x => x.ObstacleIds)
            .NotNull().WithMessage("Obstacle list cannot be null.");
    }
}

public class UpdateRaceDtoValidator : AbstractValidator<UpdateRaceDto>
{
    public UpdateRaceDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Race name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required.")
            .MaximumLength(100);

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Race date is required.");

        RuleFor(x => x.RegistrationDeadLine)
            .NotEmpty().WithMessage("Registration deadline is required.")
            .LessThanOrEqualTo(x => x.Date.AddDays(-7))
            .WithMessage("Registration deadline must be at least 7 days before the race date.");

        RuleFor(x => x.Distance)
            .InclusiveBetween(0.1, 1000.0).WithMessage("Distance must be between 0.1 and 1000 km.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(200);

        RuleFor(x => x.MaxParticipants)
            .InclusiveBetween(1, 10000).WithMessage("Max participants must be between 1 and 10000.");

        RuleFor(x => x.ImageUrl)
            .Must(uri => string.IsNullOrEmpty(uri) || Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("Image URL format is not valid.");

        RuleFor(x => x.Difficulty)
            .IsInEnum().WithMessage("Invalid difficulty level.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid race status.");
    }
}