namespace NRedberry.Numbers.Parser;

/*
 * Original: ./core/src/main/java/cc/redberry/core/number/parser/BracketToken.java
 */

public sealed class BracketToken<T> : INumberTokenParser<T>
    where T : NRedberry.INumber<T>
{
    public static BracketToken<T> Instance { get; } = new();

    private BracketToken()
    {
    }

    public T Parse(string expression, NumberParser<T> parser)
    {
        ArgumentNullException.ThrowIfNull(expression);
        ArgumentNullException.ThrowIfNull(parser);

        if (expression.Length >= 2 && expression[0] == '(' && expression[^1] == ')')
        {
            var expressionChars = expression.ToCharArray();
            var level = 0;
            foreach (var c in expressionChars)
            {
                if (c == '(')
                {
                    level++;
                }

                if (level < 1)
                {
                    return default!;
                }

                if (c == ')')
                {
                    level--;
                }
            }

            return parser.Parse(expression.Substring(1, expression.Length - 2));
        }

        return default!;
    }
}
