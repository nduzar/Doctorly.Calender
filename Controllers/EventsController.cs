using Doctorly.Calendar.Core.Dtos;
using Doctorly.Calendar.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Doctorly.Calendar.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly ICalendarService _calendarService;

    public EventsController(ICalendarService calendarService)
    {
        _calendarService = calendarService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventResponse>>> GetEvents() =>
        Ok(await _calendarService.GetAllEventsAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<EventResponse>> GetEvent(Guid id)
    {
        var ev = await _calendarService.GetEventByIdAsync(id);
        return ev == null ? NotFound() : Ok(ev);
    }

    [HttpPost]
    public async Task<ActionResult<EventResponse>> CreateEvent(CreateEventRequest request)
    {
        // Senior Note: No try-catch needed! 
        // If validation fails or a DomainException occurs, the Middleware handles it.
        var result = await _calendarService.CreateEventAsync(request);
        return CreatedAtAction(nameof(GetEvent), new { id = result.Id }, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        await _calendarService.DeleteEventAsync(id);
        return NoContent();
    }
}