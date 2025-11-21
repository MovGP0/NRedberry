namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserIndices.java
 */

public sealed class ParserIndices : ITokenParser
{
    public static ParserIndices Instance { get; } = new();

    private ParserIndices()
    {
    }

    public ParseToken? ParseToken(string expression, Parser parser)
    {
        throw new NotImplementedException();
    }

    public int Priority => throw new NotImplementedException();
}
