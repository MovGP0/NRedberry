using NRedberry.Numbers;
using NRedberry.Numbers.Parser;

namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserNumber.java
 */

public sealed class ParserNumber : ITokenParser
{
    private const int ParserPriority = 9999;

    public static ParserNumber Instance { get; } = new();

    private ParserNumber()
    {
    }

    public ParseToken? ParseToken(string expression, Parser parser)
    {
        Complex value;
        try
        {
            value = NumberParser<Complex>.ComplexParser.Parse(expression);
        }
        catch (FormatException)
        {
            return null;
        }

        return new ParseTokenNumber(value);
    }

    public int Priority => ParserPriority;
}
