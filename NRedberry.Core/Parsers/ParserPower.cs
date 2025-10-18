using System;

namespace NRedberry.Core.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserPower.java
 */

public sealed class ParserPower : ITokenParser
{
    public static ParserPower Instance { get; } = new();

    private ParserPower()
    {
    }

    public ParseToken? ParseToken(string expression, Parser parser)
    {
        throw new NotImplementedException();
    }

    public int Priority => throw new NotImplementedException();
}
