namespace NRedberry.Core.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserNumber.java
 */

public sealed class ParserNumber : ITokenParser
{
    public static ParserNumber Instance { get; } = new();

    private ParserNumber()
    {
    }

    public ParseToken? ParseToken(string expression, Parser parser)
    {
        throw new NotImplementedException();
    }

    public int Priority => throw new NotImplementedException();
}
