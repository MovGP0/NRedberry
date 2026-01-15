using System.Text.RegularExpressions;
using NRedberry.Indices;

namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserSimpleTensor.java
 */

public sealed class ParserSimpleTensor : ITokenParser
{
    private const int ParserPriority = 0;

    public static ParserSimpleTensor Instance { get; } = new();

    private ParserSimpleTensor()
    {
    }

    public ParseToken? ParseToken(string expression, Parser parser)
    {
        expression = Regex.Replace(expression, "\\{[\\s]*\\}", string.Empty);

        int indicesBegin = expression.IndexOf('_', StringComparison.Ordinal);
        int upperIndex = expression.IndexOf('^', StringComparison.Ordinal);
        if (indicesBegin < 0 && upperIndex >= 0)
        {
            indicesBegin = upperIndex;
        }

        if (indicesBegin >= 0 && upperIndex >= 0)
        {
            indicesBegin = Math.Min(indicesBegin, upperIndex);
        }

        if (indicesBegin < 0)
        {
            indicesBegin = expression.Length;
        }

        string name = expression.Substring(0, indicesBegin);
        if (name.Length == 0)
        {
            throw new ParserException("Simple tensor with empty name.");
        }

        SimpleIndices indices = parser.AllowSameVariance
            ? ParserIndices.ParseSimpleIgnoringVariance(expression.Substring(indicesBegin))
            : ParserIndices.ParseSimple(expression.Substring(indicesBegin));

        return new ParseTokenSimpleTensor(indices, name);
    }

    public int Priority => ParserPriority;
}
