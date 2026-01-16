using System.ComponentModel.DataAnnotations;

namespace Doctorly.Calendar.Core.Entities;

/// <summary>
/// Domain Entity.
/// Represents a participant in a medical event. 
/// We use 'init' for ID to ensure it's set once and never changed (Immutability).
/// </summary>
public class Attendee
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)] // Requirement: Sensible limits
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    public bool IsAttending { get; set; }
}