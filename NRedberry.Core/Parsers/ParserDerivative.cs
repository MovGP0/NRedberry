using System.Text;

namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserDerivative.java
 */

public sealed class ParserDerivative : ITokenParser
{
    private const int ParserPriority = 8000;

    public static ParserDerivative Instance { get; } = new();

    private ParserDerivative()
    {
    }

    public ParseToken? ParseToken(string expression, Parser parser)
    {
        if (!expression.Contains("][", StringComparison.Ordinal)
            || !expression.StartsWith("D[", StringComparison.Ordinal)
            || expression[^1] != ']')
        {
            return null;
        }

        var parts = expression.Split(["]["], StringSplitOptions.None);
        if (parts.Length != 2)
        {
            return null;
        }

        var argStr = parts[1].Substring(0, parts[1].Length - 1);
        if (!ParseUtils.CheckBracketsConsistence(argStr))
        {
            return null;
        }

        var arg = parser.Parse(argStr);
        if (arg is null)
        {
            return null;
        }

        var chars = parts[0].Substring(2).ToCharArray();
        var levels = new int[2];
        var buffer = new StringBuilder();
        List<ParseToken> tokens =
        [
            arg
        ];

        for (var i = 0; i < chars.Length; ++i)
        {
            if (levels[0] < 0 || levels[1] < 0)
            {
                return null;
            }

            var c = chars[i];
            if (c == '(')
            {
                ++levels[0];
            }
            else if (c == '[')
            {
                ++levels[1];
            }
            else if (c == ')')
            {
                --levels[0];
            }
            else if (c == ']')
            {
                --levels[1];
            }
            else if (c == ',' && levels[0] == 0 && levels[1] == 0)
            {
                tokens.Add(parser.Parse(buffer.ToString()));
                buffer = new StringBuilder();
            }
            else
            {
                buffer.Append(c);
            }
        }

        if (levels[0] != 0 || levels[1] != 0)
        {
            return null;
        }

        tokens.Add(parser.Parse(buffer.ToString()));
        return new ParseTokenDerivative(TokenType.Derivative, tokens.ToArray());
    }

    public int Priority => ParserPriority;
}
