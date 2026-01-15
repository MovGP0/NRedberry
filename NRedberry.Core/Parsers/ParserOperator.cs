using System.Text;

namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserOperator.java
 */

public abstract class ParserOperator : ITokenParser
{
    private readonly char _operatorSymbol;
    private readonly char _operatorInverseSymbol;

    protected ParserOperator(char operatorSymbol, char operatorInverseSymbol)
    {
        _operatorSymbol = operatorSymbol;
        _operatorInverseSymbol = operatorInverseSymbol;
    }

    public abstract int Priority { get; }

    protected bool CanParse(string expression)
    {
        var expressionChars = expression.ToCharArray();
        var level = 0;

        for (var i = 0; i < expressionChars.Length; ++i)
        {
            var c = expressionChars[i];

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
                throw new BracketsError();
            }

            if (c == _operatorSymbol && level == 0 && TestOperator(expressionChars, i))
            {
                return true;
            }

            if (c == _operatorInverseSymbol && level == 0)
            {
                return true;
            }
        }

        return false;
    }

    protected abstract ParseToken Compile(IReadOnlyList<ParseToken> nodes);

    protected abstract ParseToken InverseOperation(ParseToken tensor);

    protected virtual bool TestOperator(char[] expressionChars, int position)
    {
        return true;
    }

    protected enum Mode
    {
        Direct,
        Inverse
    }

    public virtual ParseToken? ParseToken(string expression, Parser parser)
    {
        if (!CanParse(expression))
        {
            return null;
        }

        expression = expression.Replace("--", "+", StringComparison.Ordinal);
        expression = expression.Replace("++", "+", StringComparison.Ordinal);
        expression = expression.Replace("+-", "-", StringComparison.Ordinal);
        expression = expression.Replace("-+", "-", StringComparison.Ordinal);

        var expressionChars = expression.ToCharArray();
        var buffer = new StringBuilder();
        List<ParseToken> nodes = [];
        var level = 0;
        var indicesLevel = 0;
        var mode = Mode.Direct;

        for (var i = 0; i < expressionChars.Length; ++i)
        {
            var c = expressionChars[i];

            if (c == '(' || c == '[')
            {
                level++;
            }

            if (c == '{')
            {
                indicesLevel++;
            }

            if (c == '}')
            {
                indicesLevel--;
            }

            if (c == ')' || c == ']')
            {
                level--;
            }

            if (level < 0)
            {
                throw new BracketsError(expression);
            }

            if (c == ' ' && indicesLevel == 0)
            {
                continue;
            }

            if (c == _operatorSymbol && level == 0 && TestOperator(expressionChars, i))
            {
                var toParse = buffer.ToString();
                if (!string.IsNullOrEmpty(toParse))
                {
                    ModeParser(toParse, mode, parser, nodes);
                }

                buffer = new StringBuilder();
                mode = Mode.Direct;
            }
            else if (c == _operatorInverseSymbol && level == 0)
            {
                var toParse = buffer.ToString();
                if (!string.IsNullOrEmpty(toParse))
                {
                    ModeParser(toParse, mode, parser, nodes);
                }

                buffer = new StringBuilder();
                mode = Mode.Inverse;
            }
            else
            {
                buffer.Append(c);
            }
        }

        ModeParser(buffer.ToString(), mode, parser, nodes);
        return Compile(nodes);
    }

    private void ModeParser(string expression, Mode mode, Parser parser, List<ParseToken> nodes)
    {
        if (mode == Mode.Direct)
        {
            nodes.Add(parser.Parse(expression));
            return;
        }

        if (mode == Mode.Inverse)
        {
            nodes.Add(InverseOperation(parser.Parse(expression)));
            return;
        }

        throw new ParserException("unrepoted operator parser mode");
    }
}
