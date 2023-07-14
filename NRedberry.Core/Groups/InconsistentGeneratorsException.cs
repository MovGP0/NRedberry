using System;
using System.Runtime.Serialization;

namespace NRedberry.Core.Groups;

public class InconsistentGeneratorsException : Exception
{
    public InconsistentGeneratorsException(string message) : base(message)
    {
    }

    public InconsistentGeneratorsException()
    {
    }

    public InconsistentGeneratorsException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected InconsistentGeneratorsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}