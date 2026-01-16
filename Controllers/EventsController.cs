using Doctorly.Calendar.Core.Dtos;
using Doctorly.Calendar.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Doctorly.Calendar.Controllers;

[ApiController]
[Route("api/[controller]")] // This makes the URL: api/events
public class EventsController : ControllerBase
{
    private readonly ICalendarService _calendarService;

    public EventsController(ICalendarService calendarService)
    {
        _calendarService = calendarService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventResponse>>> GetEvents()
    {
        var events = await _calendarService.GetAllEventsAsync();
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventResponse>> GetEvent(Guid id)
    {
        var ev = await _calendarService.GetEventByIdAsync(id);

        if (ev == null)
            return NotFound();

        return Ok(ev);
    }

    [HttpPost]
    public async Task<ActionResult<EventResponse>> CreateEvent(CreateEventRequest request)
    {
        try
        {
            var result = await _calendarService.CreateEventAsync(request);

            // Senior Tip: Return 201 Created with the location of the new resource
            return CreatedAtAction(nameof(GetEvent), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            // For now, we return BadRequest. Later, we'll add Global Exception Handling.
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        await _calendarService.DeleteEventAsync(id);
        return NoContent(); // 204 No Content is standard for successful deletes
    }
}