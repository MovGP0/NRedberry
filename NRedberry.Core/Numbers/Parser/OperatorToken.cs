using System;

namespace NRedberry.Core.Numbers.Parser;

/*
 * Original: ./core/src/main/java/cc/redberry/core/number/parser/OperatorToken.java
 */

public abstract class OperatorToken<T> : INumberTokenParser<T>
{
    protected OperatorToken(char operationSymbol, char operationInverseSymbol)
    {
        throw new NotImplementedException();
    }

    public T Parse(string expression, NumberParser<T> parser)
    {
        throw new NotImplementedException();
    }

    protected abstract T Neutral();

    protected abstract T Operation(T c1, T c2, bool mode);
}
