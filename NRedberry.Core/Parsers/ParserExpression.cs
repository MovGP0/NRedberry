namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserExpression.java
 */

public sealed class ParserExpression : ITokenParser
{
    private const int ParserPriority = 10100;

    public static ParserExpression Instance { get; } = new();

    private ParserExpression()
    {
    }

    public ParseToken? ParseToken(string expression, Parser parser)
    {
        if (!expression.Contains('='))
        {
            return null;
        }

        if (expression.IndexOf('=') != expression.LastIndexOf('='))
        {
            throw new ParserException("Several '=' symbols.");
        }

        var parts = expression.Split('=');
        parts[0] = parts[0].Trim();

        var preprocessing = false;
        if (parts[0][^1] == ':')
        {
            preprocessing = true;
            parts[0] = parts[0].Substring(0, parts[0].Length - 1);
        }

        ParseToken left = parser.Parse(parts[0]);
        ParseToken right = parser.Parse(parts[1]);

        return new ParseTokenExpression(preprocessing, left, right);
    }

    public int Priority => ParserPriority;
}
