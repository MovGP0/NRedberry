namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserExpression.java
 */

public sealed class ParserExpression : ITokenParser
{
    public static ParserExpression Instance { get; } = new();

    private ParserExpression()
    {
    }

    public ParseToken? ParseToken(string expression, Parser parser)
    {
        throw new NotImplementedException();
    }

    public int Priority => throw new NotImplementedException();
}
