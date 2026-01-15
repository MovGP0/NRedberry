namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserPower.java
 */

public sealed class ParserPower : ITokenParser
{
    private const string PowerLiteral = "Power";
    private const int MinimumLength = 7;

    public static ParserPower Instance { get; } = new();

    private ParserPower()
    {
    }

    public ParseToken? ParseToken(string expression, Parser parser)
    {
        if (expression.Length <= MinimumLength)
        {
            return null;
        }

        if (!expression.StartsWith(PowerLiteral + "[", StringComparison.Ordinal)
            || expression[^1] != ']')
        {
            return null;
        }

        var level = 0;
        var comma = -1;
        for (var i = PowerLiteral.Length; i < expression.Length; ++i)
        {
            var c = expression[i];
            if (c == '[')
            {
                ++level;
            }

            if (level < 1)
            {
                return null;
            }

            if (c == ']')
            {
                --level;
            }

            if (c == ',' && level == 1)
            {
                if (comma != -1)
                {
                    throw new ParserException("Power takes only two arguments.");
                }

                comma = i;
            }
        }

        ParseToken arg = parser.Parse(expression.Substring(PowerLiteral.Length + 1, comma - PowerLiteral.Length - 1));
        ParseToken power = parser.Parse(expression.Substring(comma + 1, expression.Length - comma - 2));
        return new ParseToken(TokenType.Power, arg, power);
    }

    public int Priority => 9986;
}
