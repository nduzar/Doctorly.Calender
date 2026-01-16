using Doctorly.Calendar.Core.Dtos;

namespace Doctorly.Calendar.Core.Interfaces;

/// <summary>
/// Senior Concept: Interface Abstraction.
/// Defines the 'Contract' for calendar operations without coupling to a specific database.
/// </summary>
public interface ICalendarService
{
    Task<EventResponse> CreateEventAsync(CreateEventRequest request);
    Task<IEnumerable<EventResponse>> GetAllEventsAsync();
    Task<EventResponse?> GetEventByIdAsync(Guid id);
    Task DeleteEventAsync(Guid id);
}