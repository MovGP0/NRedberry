using System;
using System.Collections.Generic;

namespace NRedberry.Core.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserProduct.java
 */

public sealed class ParserProduct : ParserOperator
{
    public static ParserProduct Instance { get; } = new();

    private ParserProduct()
        : base('*', '/')
    {
    }

    public override ParseToken? ParseToken(string expression, Parser parser)
    {
        throw new NotImplementedException();
    }

    public override int Priority => throw new NotImplementedException();

    protected override ParseToken Compile(IReadOnlyList<ParseToken> nodes)
    {
        throw new NotImplementedException();
    }

    protected override ParseToken InverseOperation(ParseToken tensor)
    {
        throw new NotImplementedException();
    }

    protected override bool TestOperator(char[] expressionChars, int position)
    {
        throw new NotImplementedException();
    }
}
