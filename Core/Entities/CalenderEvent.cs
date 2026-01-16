using System.ComponentModel.DataAnnotations;
using Doctorly.Calendar.Core.Exceptions;

namespace Doctorly.Calendar.Core.Entities;

/// <summary>
/// Domain Entity (Aggregate Root).
/// Manages the schedule and attendees for a doctor's practice event.
/// Enforces business rules like valid time ranges and concurrency.
/// </summary>
public class CalendarEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    [MaxLength(200)] // Requirement: Sensible limits
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    // Requirement: Collection of attendees
    public List<Attendee> Attendees { get; private set; } = new();

    /// <summary>
    /// Requirement (Could): Concurrency check.
    /// This Guid is updated on every save to prevent simultaneous update conflicts.
    /// </summary>
    [ConcurrencyCheck]
    public Guid Version { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Updates the event schedule with validation.
    /// Senior Note: Logic lives here to prevent an 'Anemic Domain Model'.
    /// </summary>
    public void SetSchedule(DateTime start, DateTime end)
    {
        if (end <= start)
            throw new DomainException("The event must end after it starts.");

        StartTime = start;
        EndTime = end;
        Version = Guid.NewGuid(); // Update version to track change
    }

    /// <summary>
    /// Adds a new attendee and ensures no duplicate emails exist for this event.
    /// </summary>
    public void AddAttendee(Attendee attendee)
    {
        var exists = Attendees.Any(a => a.Email.Equals(attendee.Email, StringComparison.OrdinalIgnoreCase));

        if (exists)
            throw new DomainException($"Attendee with email {attendee.Email} is already invited.");

        Attendees.Add(attendee);
        Version = Guid.NewGuid(); // Update version to track change
    }
}