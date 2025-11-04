namespace NRedberry.Core.Exceptions;

/// <summary>
/// Exception thrown when inconsistent generators are detected.
/// </summary>
public class InconsistentGeneratorsException : Exception
{
    public InconsistentGeneratorsException(string message)
        : base(message)
    {
    }

    public InconsistentGeneratorsException()
    {
    }

    public InconsistentGeneratorsException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
