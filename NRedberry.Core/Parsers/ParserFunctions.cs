using System;

namespace NRedberry.Core.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserFunctions.java
 */

public sealed class ParserFunctions : ITokenParser
{
    public static ParserFunctions Instance { get; } = new();

    private ParserFunctions()
    {
    }

    public ParseToken? ParseToken(string expression, Parser parser)
    {
        throw new NotImplementedException();
    }

    public int Priority => throw new NotImplementedException();
}
