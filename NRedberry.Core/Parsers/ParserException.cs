using System;

namespace NRedberry.Core.Parsers
{
    public class ParserException : Exception
    {
        public ParserException(string message): base(message)
        {
        }

        public ParserException()
        {
        }

        public ParserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ParserException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}