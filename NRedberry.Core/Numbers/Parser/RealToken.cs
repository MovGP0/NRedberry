namespace NRedberry.Core.Numbers.Parser;

/*
 * Original: ./core/src/main/java/cc/redberry/core/number/parser/RealToken.java
 */

public sealed class RealToken : INumberTokenParser<Real>
{
    public static RealToken Instance { get; } = new();

    private RealToken()
    {
    }

    public Real Parse(string expression, NumberParser<Real> parser)
    {
        throw new NotImplementedException();
    }
}
