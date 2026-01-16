using FluentValidation;
using Doctorly.Calendar.Core.Dtos;

namespace Doctorly.Calendar.Features.Events;

/// <summary>
/// Senior Concept: Fluent Validation.
/// Decouples validation logic from the DTO and Service layers.
/// Fulfills the "Sensible Limits" requirement.
/// </summary>
public class CreateEventRequestValidator : AbstractValidator<CreateEventRequest>
{
    public CreateEventRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required.")
            .GreaterThan(DateTime.UtcNow).WithMessage("Event cannot be scheduled in the past.");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End time is required.")
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after the start time.");

        // Validating the collection of attendees
        RuleForEach(x => x.Attendees).ChildRules(attendee =>
        {
            attendee.RuleFor(a => a.Name)
                .NotEmpty().WithMessage("Attendee name is required.")
                .MaximumLength(100).WithMessage("Name is too long.");

            attendee.RuleFor(a => a.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");
        });
    }
}