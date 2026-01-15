using System.Globalization;
using System.Numerics;

namespace NRedberry.Numbers.Parser;

/*
 * Original: ./core/src/main/java/cc/redberry/core/number/parser/RealToken.java
 */

public sealed class RealToken : INumberTokenParser<Real>
{
    public static RealToken Instance { get; } = new();

    private RealToken()
    {
    }

    public Real Parse(string expression, NumberParser<Real> parser)
    {
        ArgumentNullException.ThrowIfNull(expression);
        ArgumentNullException.ThrowIfNull(parser);

        if (BigInteger.TryParse(expression, NumberStyles.Integer, CultureInfo.InvariantCulture, out var bigInteger))
        {
            return new Rational(bigInteger);
        }

        if (double.TryParse(expression, NumberStyles.Float, CultureInfo.InvariantCulture, out var doubleValue))
        {
            return new NRedberry.Numeric(doubleValue);
        }

        return null!;
    }
}
