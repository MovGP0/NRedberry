using System;
using System.Runtime.Serialization;

namespace NRedberry.Core.Combinatorics
{
    public sealed class InconsistentGeneratorsException : ApplicationException
    {
        public InconsistentGeneratorsException()
        {
        }

        public InconsistentGeneratorsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InconsistentGeneratorsException(string message) : base(message)
        {
        }

        public InconsistentGeneratorsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
