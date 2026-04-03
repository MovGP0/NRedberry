namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserPowerAst.java
 */

public sealed class ParserPowerAst : ITokenParser
{
    public static ParserPowerAst Instance { get; } = new();

    private ParserPowerAst()
    {
    }

    public ParseToken? ParseToken(string expression, Parser parser)
    {
        var expressionChars = expression.ToCharArray();
        var level = 0;
        string? argumentExpression = null;
        string? powerExpression = null;

        for (var i = 0; i < expressionChars.Length; ++i)
        {
            var c = expressionChars[i];

            if (c == '(' || c == '[')
            {
                ++level;
            }

            if (c == ')' || c == ']')
            {
                --level;
            }

            if (level < 0)
            {
                throw new BracketsError();
            }

            if (c == '*'
                && level == 0
                && i + 1 < expressionChars.Length
                && expressionChars[i + 1] == '*')
            {
                argumentExpression = expression.Substring(0, i);
                powerExpression = expression.Substring(i + 2);
                break;
            }
        }

        if (argumentExpression is null || powerExpression is null)
        {
            return null;
        }

        var argument = parser.Parse(argumentExpression);
        var power = parser.Parse(powerExpression);
        return new ParseToken(TokenType.Power, argument, power);
    }

    public int Priority => 990;
}
