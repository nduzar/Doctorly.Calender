using Doctorly.Calendar.Core.Dtos;
using Doctorly.Calendar.Core.Entities;
using Doctorly.Calendar.Core.Interfaces;
using Doctorly.Calendar.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Doctorly.Calendar.Features.Events;

/// <summary>
/// Service implementation for managing calendar events.
/// Logic is coordinated here between the DB context and Domain Entities.
/// </summary>
public class CalendarService : ICalendarService
{
    private readonly AppDbContext _context;

    public CalendarService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EventResponse> CreateEventAsync(CreateEventRequest request)
    {
        // 1. Initialize the Aggregate Root
        var newEvent = new CalendarEvent
        {
            Title = request.Title,
            Description = request.Description ?? string.Empty
        };

        // 2. Use Domain Logic to set the schedule (enforces validation)
        newEvent.SetSchedule(request.StartTime, request.EndTime);

        // 3. Map and add Attendees using the Domain method
        foreach (var a in request.Attendees)
        {
            newEvent.AddAttendee(new Attendee
            {
                Name = a.Name,
                Email = a.Email,
                IsAttending = a.IsAttending
            });
        }

        // 4. Persist to SQLite
        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();

        return MapToResponse(newEvent);
    }

    public async Task<IEnumerable<EventResponse>> GetAllEventsAsync()
    {
        var events = await _context.Events
            .Include(e => e.Attendees)
            .AsNoTracking() // Senior Tip: Read-only performance optimization
            .ToListAsync();

        return events.Select(MapToResponse);
    }

    public async Task<EventResponse?> GetEventByIdAsync(Guid id)
    {
        var ev = await _context.Events
            .Include(e => e.Attendees)
            .FirstOrDefaultAsync(e => e.Id == id);

        return ev == null ? null : MapToResponse(ev);
    }

    public async Task DeleteEventAsync(Guid id)
    {
        var ev = await _context.Events.FindAsync(id);
        if (ev != null)
        {
            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Private mapping method to keep code DRY (Don't Repeat Yourself).
    /// Converts Domain Entity to DTO.
    /// </summary>
    private static EventResponse MapToResponse(CalendarEvent ev)
    {
        return new EventResponse(
            ev.Id,
            ev.Title,
            ev.Description,
            ev.StartTime,
            ev.EndTime,
            ev.Attendees.Select(a => new AttendeeDto(a.Name, a.Email, a.IsAttending)).ToList(),
            ev.Version
        );
    }
}