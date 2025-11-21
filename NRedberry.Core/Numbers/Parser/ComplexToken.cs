namespace NRedberry.Numbers.Parser;

/*
 * Original: ./core/src/main/java/cc/redberry/core/number/parser/ComplexToken.java
 */

public sealed class ComplexToken : INumberTokenParser<Complex>
{
    public static ComplexToken Instance { get; } = new();

    private ComplexToken()
    {
    }

    public Complex Parse(string expression, NumberParser<Complex> parser)
    {
        throw new NotImplementedException();
    }
}
