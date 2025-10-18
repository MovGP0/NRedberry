using System;
using NRedberry.Core.Numbers;

namespace NRedberry.Core.Numbers.Parser;

/*
 * Original: ./core/src/main/java/cc/redberry/core/number/parser/NumberParser.java
 */

public sealed class NumberParser<T>
{
    public static NumberParser<Real> RealParser => throw new NotImplementedException();

    public static NumberParser<Complex> ComplexParser => throw new NotImplementedException();

    public NumberParser(INumberTokenParser<T>[] parsers)
    {
        throw new NotImplementedException();
    }

    public T Parse(string expression)
    {
        throw new NotImplementedException();
    }
}
