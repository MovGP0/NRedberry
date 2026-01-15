using NRedberry.Indices;
using NRedberry.Numbers;

namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserTensorField.java
 */

public sealed class ParserTensorField : ITokenParser
{
    private const int ParserPriority = 7000;

    public static ParserTensorField Instance { get; } = new();

    private ParserTensorField()
    {
    }

    public int Priority => ParserPriority;

    public ParseToken? ParseToken(string expression, Parser parser)
    {
        if (!expression.Contains('[', StringComparison.Ordinal))
        {
            return null;
        }

        if (!CanParse(expression))
        {
            return null;
        }

        int openIndex = expression.IndexOf('[', StringComparison.Ordinal);
        string tensorPart = expression.Substring(0, openIndex);
        var simpleTensorNode = ParserSimpleTensor.Instance.ParseToken(tensorPart, parser) as ParseTokenSimpleTensor;
        if (simpleTensorNode is null)
        {
            return null;
        }

        string argString = expression[(openIndex + 1)..^1];
        List<ParseToken> arguments = [];
        List<SimpleIndices?> indices = [];

        int beginIndex = 0;
        int level = 0;
        char[] argsChars = argString.ToCharArray();
        for (int i = 0; i < argsChars.Length; ++i)
        {
            char c = argsChars[i];

            if ((c == ',' && level == 0) || i == argsChars.Length - 1)
            {
                int length = i == argsChars.Length - 1 ? i + 1 - beginIndex : i - beginIndex;
                string argument = argString.Substring(beginIndex, length);
                string[] split = argument.Split(':', StringSplitOptions.None);
                ParseToken a;
                SimpleIndices? aIndices;

                if (split.Length == 1)
                {
                    a = parser.Parse(argument);
                    aIndices = null;
                }
                else
                {
                    if (split.Length != 2)
                    {
                        throw new ParserException(expression);
                    }

                    a = parser.Parse(split[0]);
                    aIndices = ParserIndices.ParseSimple(split[1]);
                }

                arguments.Add(a);
                indices.Add(aIndices);
                beginIndex = i + 1;
            }

            if (c == '[')
            {
                ++level;
            }

            if (c == ']')
            {
                --level;
            }
        }

        if (string.Equals(simpleTensorNode.Name, "sqrt", StringComparison.OrdinalIgnoreCase)
            && simpleTensorNode.Indices.Size() == 0
            && arguments.Count == 1
            && arguments[0].GetIndices().GetFree().Size() == 0)
        {
            return new ParseToken(TokenType.Power, arguments[0], new ParseTokenNumber(Complex.OneHalf));
        }

        if (string.Equals(simpleTensorNode.Name, "tr", StringComparison.OrdinalIgnoreCase))
        {
            return new ParseToken(TokenType.Trace, arguments.ToArray());
        }

        var argumentsIndices = new SimpleIndices[indices.Count];
        for (int i = 0; i < indices.Count; ++i)
        {
            argumentsIndices[i] = indices[i]!;
        }

        return new ParseTokenTensorField(
            simpleTensorNode.Indices,
            simpleTensorNode.Name,
            arguments.ToArray(),
            argumentsIndices);
    }

    private static bool CanParse(string expression)
    {
        char[] expressionChars = expression.ToCharArray();
        int level = 0;
        bool inBrackets = false;
        foreach (char c in expressionChars)
        {
            if ((c == '+' || c == '*' || c == '-' || c == '/') && !inBrackets)
            {
                return false;
            }

            if (c == '[')
            {
                inBrackets = true;
                level++;
            }

            if (c == ']')
            {
                level--;
            }

            if (level == 0)
            {
                inBrackets = false;
            }

            if (level < 0)
            {
                throw new BracketsError(expression);
            }
        }

        return true;
    }
}
