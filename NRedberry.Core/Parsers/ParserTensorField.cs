namespace NRedberry.Core.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserTensorField.java
 */

public sealed class ParserTensorField : ITokenParser
{
    public static ParserTensorField Instance { get; } = new();

    private ParserTensorField()
    {
    }

    public ParseToken? ParseToken(string expression, Parser parser)
    {
        throw new NotImplementedException();
    }

    public int Priority => throw new NotImplementedException();
}
