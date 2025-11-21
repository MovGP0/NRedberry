namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/BracketsError.java
 */

public class BracketsError : ParserException
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
