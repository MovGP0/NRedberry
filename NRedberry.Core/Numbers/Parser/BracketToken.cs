namespace NRedberry.Core.Numbers.Parser;

/*
 * Original: ./core/src/main/java/cc/redberry/core/number/parser/BracketToken.java
 */

public sealed class BracketToken<T> : INumberTokenParser<T>
{
    public static BracketToken<T> Instance { get; } = new();

    private BracketToken()
    {
    }

    public T Parse(string expression, NumberParser<T> parser)
    {
        throw new NotImplementedException();
    }
}
