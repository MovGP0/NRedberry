using System.Text;

namespace NRedberry.Numbers.Parser;

/*
 * Original: ./core/src/main/java/cc/redberry/core/number/parser/OperatorToken.java
 */

public abstract class OperatorToken<T>(char operationSymbol, char operationInverseSymbol) : INumberTokenParser<T>
    where T : NRedberry.INumber<T>
{
    public T Parse(string expression, NumberParser<T> parser)
    {
        ArgumentNullException.ThrowIfNull(expression);
        ArgumentNullException.ThrowIfNull(parser);

        if (!CanParse(expression))
        {
            return default!;
        }

        var buffer = new StringBuilder();
        var temp = Neutral();
        var level = 0;
        var mode = false;
        foreach (var c in expression)
        {
            if (c == '(')
            {
                level++;
            }

            if (c == ')')
            {
                level--;
            }

            if (level < 0)
            {
                throw new BracketsError();
            }

            if (c == operationSymbol && level == 0)
            {
                if (buffer.Length != 0)
                {
                    temp = Operation(temp, parser.Parse(buffer.ToString()), mode);
                }

                buffer.Clear();
                mode = false;
            }
            else if (c == operationInverseSymbol && level == 0)
            {
                if (buffer.Length != 0)
                {
                    temp = Operation(temp, parser.Parse(buffer.ToString()), mode);
                }

                buffer.Clear();
                mode = true;
            }
            else
            {
                buffer.Append(c);
            }
        }

        if (temp is null)
        {
            temp = Neutral();
        }

        temp = Operation(temp, parser.Parse(buffer.ToString()), mode);
        return temp;
    }

    private bool CanParse(string expression)
    {
        if (expression.Contains("**", StringComparison.Ordinal))
        {
            return false;
        }

        var level = 0;
        foreach (var c in expression)
        {
            if (c == '(' || c == '[')
            {
                level++;
            }

            if (c == ')' || c == ']')
            {
                level--;
            }

            if (level < 0)
            {
                throw new BracketsError(expression);
            }

            if ((c == operationSymbol || c == operationInverseSymbol) && level == 0)
            {
                return true;
            }
        }

        return false;
    }

    protected abstract T Neutral();

    protected abstract T Operation(T c1, T c2, bool mode);
}
