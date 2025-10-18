using System;

namespace NRedberry.Core.Numbers.Parser;

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
}
