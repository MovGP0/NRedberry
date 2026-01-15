namespace NRedberry.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserFunctions.java
 */

public sealed class ParserFunctions : ITokenParser
{
    private static readonly string[] Functions =
    [
        "Sin",
        "Cos",
        "Tan",
        "Log",
        "Exp",
        "Cot",
        "ArcSin",
        "ArcCos",
        "ArcTan",
        "ArcCot"
    ];

    public static ParserFunctions Instance { get; } = new();

    private ParserFunctions()
    {
    }

    public ParseToken? ParseToken(string expression, Parser parser)
    {
        if (!expression.Contains('[') || expression.LastIndexOf(']') != expression.Length - 1)
        {
            return null;
        }

        string? function = null;
        string? temp = null;
        for (var i = 0; i < Functions.Length; ++i)
        {
            temp = Functions[i];
            if (expression.Length - 2 < temp.Length)
            {
                continue;
            }

            if (expression.Substring(0, temp.Length) == temp)
            {
                function = temp;
                break;
            }

            temp = null;
        }

        if (function is null || temp is null)
        {
            return null;
        }

        if (expression[function.Length] != '[')
        {
            return null;
        }

        var level = 0;
        for (var i = temp.Length + 1; i < expression.Length - 2; ++i)
        {
            var c = expression[i];
            if (c == '[')
            {
                level++;
            }

            if (c == ']')
            {
                level--;
            }

            if (level < 0)
            {
                return null;
            }

            if (c == ',' && level == 0)
            {
                throw new ParserException("Sin, Cos, Tan and others scalar functions take only one argument.");
            }
        }

        var argument = expression.Substring(temp.Length + 1, expression.Length - temp.Length - 2);
        return new ParseTokenScalarFunction(temp, parser.Parse(argument));
    }

    public int Priority => 9987;
}
