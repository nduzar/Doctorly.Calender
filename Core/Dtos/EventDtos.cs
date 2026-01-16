using System.ComponentModel.DataAnnotations;

namespace Doctorly.Calendar.Core.Dtos;

/// <summary>
/// Data Transfer Objects for the Event API.
/// We use 'record' for immutability and clean syntax.
/// </summary>

public record AttendeeDto(
    [Required] string Name,
    [Required, EmailAddress] string Email,
    bool IsAttending);

public record CreateEventRequest(
    [Required, MaxLength(200)] string Title,
    [MaxLength(1000)] string Description,
    [Required] DateTime StartTime,
    [Required] DateTime EndTime,
    List<AttendeeDto> Attendees);

public record EventResponse(
    Guid Id,
    string Title,
    string Description,
    DateTime StartTime,
    DateTime EndTime,
    List<AttendeeDto> Attendees,
    Guid Version);