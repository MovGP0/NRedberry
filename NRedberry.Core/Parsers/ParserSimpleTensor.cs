using System;

namespace NRedberry.Core.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserSimpleTensor.java
 */

public sealed class ParserSimpleTensor : ITokenParser
{
    public static ParserSimpleTensor Instance { get; } = new();

    private ParserSimpleTensor()
    {
    }

    public ParseToken? ParseToken(string expression, Parser parser)
    {
        throw new NotImplementedException();
    }

    public int Priority => throw new NotImplementedException();
}
