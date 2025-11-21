namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserBrackets.java
 */

public sealed class ParserBrackets : ITokenParser
{
    public static ParserBrackets Instance { get; } = new();

    private ParserBrackets()
    {
    }

    public ParseToken? ParseToken(string expression, Parser parser)
    {
        throw new NotImplementedException();
    }

    public int Priority => throw new NotImplementedException();
}
