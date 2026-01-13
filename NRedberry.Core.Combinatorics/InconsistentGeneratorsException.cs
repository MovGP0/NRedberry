namespace NRedberry.Core.Combinatorics;

public sealed class InconsistentGeneratorsException : Exception
{
    public InconsistentGeneratorsException()
    {
    }

    public InconsistentGeneratorsException(string message)
        : base(message)
    {
    }

    public InconsistentGeneratorsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
