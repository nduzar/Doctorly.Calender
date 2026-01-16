namespace Doctorly.Calendar.Core.Exceptions;

/// <summary>
/// Custom exception for business rule violations.
/// Specialized exceptions allow our Global Middleware 
/// to distinguish between "User Errors" (400) and "Server Errors" (500).
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}