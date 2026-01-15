using System.Globalization;
using System.Numerics;

namespace NRedberry.Numbers.Parser;

/*
 * Original: ./core/src/main/java/cc/redberry/core/number/parser/ComplexToken.java
 */

public sealed class ComplexToken : INumberTokenParser<Complex>
{
    public static ComplexToken Instance { get; } = new();

    private ComplexToken()
    {
    }

    public Complex Parse(string expression, NumberParser<Complex> parser)
    {
        ArgumentNullException.ThrowIfNull(expression);
        ArgumentNullException.ThrowIfNull(parser);

        if (expression == "I")
        {
            return Complex.ImaginaryOne;
        }

        if (BigInteger.TryParse(expression, NumberStyles.Integer, CultureInfo.InvariantCulture, out var bigInteger))
        {
            return new Complex(bigInteger);
        }

        if (double.TryParse(expression, NumberStyles.Float, CultureInfo.InvariantCulture, out var doubleValue))
        {
            return new Complex(doubleValue);
        }

        return null!;
    }
}
