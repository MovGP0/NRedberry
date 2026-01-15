using System.Text.RegularExpressions;
using NRedberry.Indices;
using ContextCC = NRedberry.Contexts.CC;

namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserIndices.java
 */

public static class ParserIndices
{
    private const int UpperMask = unchecked((int)0x80000000);

    private static readonly Regex Pattern = new(
        "((?>(?>[a-zA-Z\\p{IsGreek}])|(?>\\\\[a-zA-Z]*))(?>_(?>(?>[0-9])|(?>[\\{][0-9\\s]*[\\}])))?[']*)",
        RegexOptions.Compiled
    );

    public static SimpleIndices ParseSimple(string expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        return IndicesFactory.CreateSimple(null, Parse(expression));
    }

    public static SimpleIndices ParseSimpleIgnoringVariance(string expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        var indices = Parse(expression);
        for (var i = 0; i < indices.Length - 1; ++i)
        {
            for (var j = i + 1; j < indices.Length; ++j)
            {
                if (indices[i] == indices[j])
                {
                    indices[i] = IndicesUtils.InverseIndexState(indices[i]);
                    break;
                }
            }
        }

        return IndicesFactory.CreateSimple(null, indices);
    }

    public static int[] Parse(string expression)
    {
        ArgumentNullException.ThrowIfNull(expression);

        if (expression.Length == 0)
        {
            return [];
        }

        List<int> indices = [];
        var level = 0;
        var state = 0;
        var expressionChars = expression.ToCharArray();
        var beginIndex = 0;
        var endIndex = 0;
        for (; endIndex < expressionChars.Length; ++endIndex)
        {
            var c = expressionChars[endIndex];
            if (c == '{')
            {
                ++level;
            }
            else if (c == '}')
            {
                --level;
            }
            else if (c == '_' && level == 0)
            {
                if (endIndex != 0)
                {
                    Parse(expression.Substring(beginIndex + 1, endIndex - beginIndex - 1), indices, state);
                }

                state = 0;
                beginIndex = endIndex;
            }
            else if (c == '^')
            {
                if (level != 0)
                {
                    throw new BracketsError();
                }

                if (endIndex != 0)
                {
                    Parse(expression.Substring(beginIndex + 1, endIndex - beginIndex - 1), indices, state);
                }

                state = UpperMask;
                beginIndex = endIndex;
            }
        }

        if (level != 0)
        {
            throw new BracketsError();
        }

        if (beginIndex != endIndex)
        {
            Parse(expression.Substring(beginIndex + 1, endIndex - beginIndex - 1), indices, state);
        }

        return indices.ToArray();
    }

    private static void Parse(string expression, List<int> indices, int state)
    {
        var matches = Pattern.Matches(expression);
        foreach (Match match in matches)
        {
            indices.Add(ContextCC.IndexConverterManager.GetCode(match.Value) | state);
        }

        var remainder = Pattern.Replace(expression, string.Empty);
        remainder = Regex.Replace(remainder, "[\\{\\}\\s]*", string.Empty);
        if (remainder.Length != 0)
        {
            throw new ParserException($"Incorrect indices: {expression}");
        }
    }
}
