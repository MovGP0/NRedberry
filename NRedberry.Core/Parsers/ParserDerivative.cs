namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserDerivative.java
 */

public sealed class ParserDerivative : ITokenParser
{
    public static ParserDerivative Instance { get; } = new();

    private ParserDerivative()
    {
    }

    public ParseToken? ParseToken(string expression, Parser parser)
    {
        throw new NotImplementedException();
    }

    public int Priority => throw new NotImplementedException();
}
