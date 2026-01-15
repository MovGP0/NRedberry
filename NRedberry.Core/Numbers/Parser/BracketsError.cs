using System.Runtime.Serialization;

namespace NRedberry.Numbers.Parser;

/*
 * Original: ./core/src/main/java/cc/redberry/core/number/parser/BracketsError.java
 */

public sealed class BracketsError : Exception
{
    public BracketsError(string message)
        : base($"Unbalanced brackets in {message}")
    {
    }

    public BracketsError()
        : base("Unbalanced brackets.")
    {
    }

    public BracketsError(string message, Exception innerException)
        : base($"Unbalanced brackets in {message}", innerException)
    {
    }

    private BracketsError(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
