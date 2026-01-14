using System.Numerics;
using NRedberry;
using NRedberry.Apache.Commons.Math;
using NRedberry.Tensors;

namespace NRedberry.Numbers;

public static class NumberUtils
{
    private static readonly BigInteger s_two = new(2);
    private static readonly BigFraction s_oneFraction = new(1, 1);

    public static BigInteger TwoBigInt { get; } = new(2);

    [Obsolete("Check for null and throw ArgumentNullException instead.")]
    public static void CheckNotNull(object o)
    {
        if (o is null)
        {
            throw new ArgumentNullException(nameof(o));
        }
    }

    public static Numeric CreateNumeric(double value)
    {
        if (value == 0.0)
        {
            return Numeric.Zero;
        }

        if (value == 1.0)
        {
            return Numeric.One;
        }

        if (double.IsPositiveInfinity(value))
        {
            return Numeric.PositiveInfinity;
        }

        if (double.IsNegativeInfinity(value))
        {
            return Numeric.NegativeInfinity;
        }

        if (double.IsNaN(value))
        {
            return Numeric.NaN;
        }

        return new Numeric(value);
    }

    public static Rational CreateRational(BigFraction fraction)
    {
        ArgumentNullException.ThrowIfNull(fraction);
        if (fraction.Numerator.IsZero)
        {
            return Rational.Zero;
        }

        if (fraction.Equals(s_oneFraction))
        {
            return Rational.One;
        }

        return new Rational(fraction.Numerator, fraction.Denominator);
    }

    /// <summary>
    /// Computes the integer square root of a number.
    /// </summary>
    /// <param name="value">The number.</param>
    /// <returns>The integer square root, i.e. the largest number whose square doesn't exceed <paramref name="value"/>.</returns>
    public static BigInteger Sqrt(BigInteger value)
    {
        if (value.Sign < 0)
        {
            throw new ArithmeticException("square root of negative number");
        }

        if (value.IsZero)
        {
            return BigInteger.Zero;
        }

        int bitLength = GetBitLength(value);
        BigInteger root = BigInteger.One << (bitLength / 2);
        while (!IsSqrtBounds(value, root))
        {
            root = (root + (value / root)) / s_two;
        }

        return root;
    }

    private static bool IsSqrtBounds(BigInteger value, BigInteger root)
    {
        BigInteger lowerBound = root * root;
        BigInteger upperBound = (root + BigInteger.One) * (root + BigInteger.One);
        return lowerBound.CompareTo(value) <= 0 && value.CompareTo(upperBound) < 0;
    }

    public static bool IsSqrt(BigInteger value, BigInteger root)
    {
        return value.CompareTo(root * root) == 0;
    }

    public static bool IsIntegerOdd(Complex complex)
    {
        ArgumentNullException.ThrowIfNull(complex);
        if (complex.IsInteger())
        {
            BigInteger value = GetBigIntValue(complex.Real);
            return value % TwoBigInt != BigInteger.Zero;
        }

        return false;
    }

    public static bool IsIntegerEven(Complex complex)
    {
        ArgumentNullException.ThrowIfNull(complex);
        if (complex.IsInteger())
        {
            BigInteger value = GetBigIntValue(complex.Real);
            return value % TwoBigInt == BigInteger.Zero;
        }

        return false;
    }

    public static bool IsZeroOrIndeterminate(Complex complex)
    {
        ArgumentNullException.ThrowIfNull(complex);
        return complex.IsZero() || complex.IsInfinite() || complex.IsNaN();
    }

    public static bool IsIndeterminate(Complex complex)
    {
        ArgumentNullException.ThrowIfNull(complex);
        return complex.IsInfinite() || complex.IsNaN();
    }

    public static bool IsRealNegative(Complex complex)
    {
        ArgumentNullException.ThrowIfNull(complex);
        return complex.IsReal() && complex.Real.SigNum() < 0;
    }

    public static bool IsRealNumerical(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        if (tensor is Complex complex && complex.IsReal())
        {
            return true;
        }

        foreach (Tensor t in tensor)
        {
            if (!IsRealNumerical(t))
            {
                return false;
            }
        }

        return true;
    }

    public static BigInteger Pow(BigInteger baseValue, BigInteger exponent)
    {
        BigInteger result = BigInteger.One;
        BigInteger currentBase = baseValue;
        BigInteger currentExponent = exponent;
        while (currentExponent.Sign > 0)
        {
            if (!currentExponent.IsEven)
            {
                result *= currentBase;
            }

            currentBase *= currentBase;
            currentExponent >>= 1;
        }

        return result;
    }

    public static long Pow(long baseValue, long exponent)
    {
        long result = 1;
        long currentBase = baseValue;
        long currentExponent = exponent;
        while (currentExponent > 0)
        {
            if (currentExponent % 2 == 1)
            {
                result *= currentBase;
            }

            currentBase *= currentBase;
            currentExponent >>= 1;
        }

        return result;
    }

    public static BigInteger Factorial(int n)
    {
        BigInteger result = BigInteger.One;
        int value = n;
        while (value != 0)
        {
            result *= value;
            --value;
        }

        return result;
    }

    private static BigInteger GetBigIntValue(Real real)
    {
        return real switch
        {
            Rational rational => rational.BigIntValue(),
            Numeric numeric => numeric.BigIntValue(),
            _ => new BigInteger(real.DoubleValue())
        };
    }

    private static int GetBitLength(BigInteger value)
    {
        if (value.Sign < 0)
        {
            value = BigInteger.Abs(value);
        }

        byte[] bytes = value.ToByteArray();
        int msb = bytes[^1];
        int bitLength = (bytes.Length - 1) * 8;
        while (msb != 0)
        {
            msb >>= 1;
            bitLength++;
        }

        return bitLength;
    }
}
