namespace NRedberry.Core.Exceptions;

public sealed class IndexConverterException : Exception
{
    public IndexConverterException()
    {
    }

    public IndexConverterException(string message)
        : base(message)
    {
    }

    public IndexConverterException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
