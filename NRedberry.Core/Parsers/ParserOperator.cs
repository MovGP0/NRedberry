using System;
using System.Collections.Generic;

namespace NRedberry.Core.Parsers;

/*
 * Original: ./core/src/main/java/cc/redberry/core/parser/ParserOperator.java
 */

public abstract class ParserOperator : ITokenParser
{
    protected ParserOperator(char operatorSymbol, char operatorInverseSymbol)
    {
        throw new NotImplementedException();
    }

    public abstract ParseToken? ParseToken(string expression, Parser parser);

    public abstract int Priority { get; }

    protected abstract ParseToken Compile(IReadOnlyList<ParseToken> nodes);

    protected abstract ParseToken InverseOperation(ParseToken tensor);

    protected virtual bool TestOperator(char[] expressionChars, int position)
    {
        throw new NotImplementedException();
    }
}
