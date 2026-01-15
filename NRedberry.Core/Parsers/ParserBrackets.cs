namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserBrackets.java
 */

public sealed class ParserBrackets : ITokenParser
{
    private const int ParserId = int.MaxValue;

    public static ParserBrackets Instance { get; } = new();

    private ParserBrackets()
    {
    }

    public ParseToken? ParseToken(string expression, Parser parser)
    {
        if (expression[0] == '(')
        {
            if (expression[^1] != ')')
            {
                CheckWithException(expression);
            }
            else
            {
                var level = 0;
                for (var i = 0; i < expression.Length; ++i)
                {
                    var c = expression[i];
                    if (c == '(')
                    {
                        level++;
                    }

                    if (level < 1)
                    {
                        return null;
                    }

                    if (c == ')')
                    {
                        level--;
                    }
                }

                if (level != 0)
                {
                    throw new BracketsError();
                }

                return parser.Parse(expression.Substring(1, expression.Length - 2));
            }
        }

        return null;
    }

    public int Priority => ParserId;

    private static void CheckWithException(string expression)
    {
        var level = 0;
        for (var i = 0; i < expression.Length; ++i)
        {
            var c = expression[i];
            if (c == '(')
            {
                level++;
            }

            if (c == ')')
            {
                level--;
            }
        }

        if (level != 0)
        {
            throw new BracketsError(expression);
        }
    }
}
