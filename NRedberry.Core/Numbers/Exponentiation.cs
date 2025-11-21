using System.Numerics;

namespace NRedberry.Numbers;

public static class Exponentiation
{
    public static Real? ExponentiateIfPossible(Real @base, Real power)
    {
        ArgumentNullException.ThrowIfNull(@base);
        ArgumentNullException.ThrowIfNull(power);

        if (@base.IsZero())
        {
            if (power.IsInfinite())
            {
                return NRedberry.Numeric.NaN;
            }

            return Rational.Zero;
        }

        if (@base.IsNumeric() || power.IsNumeric())
        {
            return new NRedberry.Numeric(Math.Pow(@base.GetNumericValue().DoubleValue(), power.GetNumericValue().DoubleValue()));
        }

        if (power.IsInteger())
        {
            return @base.Pow(((Rational)power).Numerator);
        }

        var powerNumerator = ((Rational)power).Numerator;
        var powerDenominator = ((Rational)power).Denominator;

        var baseNumerator = ((Rational)@base).Numerator;
        var baseDenominator = ((Rational)@base).Denominator;

        var baseNumeratorRoot = FindIntegerRoot(baseNumerator, powerDenominator);
        var baseDenominatorRoot = FindIntegerRoot(baseDenominator, powerDenominator);

        if (baseNumeratorRoot is null || baseDenominatorRoot is null)
        {
            return null;
        }

        return ExponentiateIfPossible(new Rational(baseNumeratorRoot.Value, baseDenominatorRoot.Value), new Rational(powerNumerator));
    }

    private static BigInteger? FindIntegerRoot(BigInteger @base, BigInteger power)
    {
        if (power.Sign <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(power), "Power must be positive.");
        }

        var exponent = ToIntExponent(power);

        if (@base.IsZero)
        {
            return BigInteger.Zero;
        }

        var isNegativeBase = @base.Sign < 0;
        if (isNegativeBase && (exponent & 0x1) == 0)
        {
            return null;
        }

        var target = BigInteger.Abs(@base);
        var low = BigInteger.Zero;
        var high = BigInteger.One;

        while (BigInteger.Pow(high, exponent).CompareTo(target) < 0)
        {
            high <<= 1;
        }

        while ((high - low).CompareTo(BigInteger.One) > 0)
        {
            var mid = (high + low) >> 1;
            var midPower = BigInteger.Pow(mid, exponent);
            var comparison = midPower.CompareTo(target);

            if (comparison == 0)
            {
                return isNegativeBase ? BigInteger.Negate(mid) : mid;
            }

            if (comparison < 0)
            {
                low = mid;
            }
            else
            {
                high = mid;
            }
        }

        if (BigInteger.Pow(low, exponent).CompareTo(target) == 0)
        {
            return isNegativeBase ? BigInteger.Negate(low) : low;
        }

        if (BigInteger.Pow(high, exponent).CompareTo(target) == 0)
        {
            return isNegativeBase ? BigInteger.Negate(high) : high;
        }

        return null;
    }

    private static int ToIntExponent(BigInteger power)
    {
        if (power > new BigInteger(int.MaxValue))
        {
            throw new InvalidOperationException("Too many bits...");
        }

        return (int)power;
    }

    public static Complex? ExponentiateIfPossible(Complex @base, Complex power)
    {
        ArgumentNullException.ThrowIfNull(@base);
        ArgumentNullException.ThrowIfNull(power);

        if (@base.IsInfinite())
        {
            if (power.IsZero())
            {
                return Complex.ComplexNaN;
            }

            return @base;
        }

        if (@base.IsOne())
        {
            if (power.IsInfinite())
            {
                return power.Multiply(@base);
            }

            return @base;
        }

        if (power.IsOne())
        {
            return @base;
        }

        if (@base.IsZero())
        {
            if (power.Real.SigNum() <= 0)
            {
                return Complex.ComplexNaN;
            }

            return @base;
        }

        if (power.IsZero())
        {
            return Complex.One;
        }

        if (@base.IsNumeric() || power.IsNumeric())
        {
            return @base.PowNumeric(power);
        }

        if (power.IsReal())
        {
            var pp = (Rational)power.Real;

            if (@base.IsReal())
            {
                var value = ExponentiateIfPossible(@base.Real, pp);
                if (value is null)
                {
                    return null;
                }

                return new Complex(value);
            }

            if (pp.IsInteger())
            {
                return @base.Pow(pp.Numerator);
            }

            var root = FindIntegerRoot(@base, pp.Denominator);
            if (root is null)
            {
                return null;
            }

            return root.Pow(pp.Numerator);
        }

        return null;
    }

    public static Complex? FindIntegerRoot(Complex @base, BigInteger power)
    {
        ArgumentNullException.ThrowIfNull(@base);

        if (@base.Real is not Rational || @base.Imaginary is not Rational)
        {
            return null;
        }

        var realDenominator = ((Rational)@base.Real).Denominator;
        var imaginaryDenominator = ((Rational)@base.Imaginary).Denominator;

        var lcm = Lcm(realDenominator, imaginaryDenominator);
        var lcmRoot = FindIntegerRoot(lcm, power);

        if (lcmRoot is null)
        {
            return null;
        }

        var scaledBase = @base.Multiply(lcm);

        var numericValue = scaledBase.Pow(1.0 / (double)power);
        var real = numericValue.Real.DoubleValue();
        var imaginary = numericValue.Imaginary.DoubleValue();

        var ceilReal = (int)Math.Ceiling(real);
        var floorReal = (int)Math.Floor(real);
        var ceilImaginary = (int)Math.Ceiling(imaginary);
        var floorImaginary = (int)Math.Floor(imaginary);

        var candidate = new Complex(ceilReal, ceilImaginary);
        if (candidate.Pow(power).Equals(scaledBase))
        {
            return candidate.Divide(lcmRoot.Value);
        }

        candidate = new Complex(floorReal, ceilImaginary);
        if (candidate.Pow(power).Equals(scaledBase))
        {
            return candidate.Divide(lcmRoot.Value);
        }

        candidate = new Complex(ceilReal, floorImaginary);
        if (candidate.Pow(power).Equals(scaledBase))
        {
            return candidate.Divide(lcmRoot.Value);
        }

        candidate = new Complex(floorReal, floorImaginary);
        if (candidate.Pow(power).Equals(scaledBase))
        {
            return candidate.Divide(lcmRoot.Value);
        }

        return null;
    }

    private static BigInteger Lcm(BigInteger left, BigInteger right)
    {
        if (left.IsZero || right.IsZero)
        {
            return BigInteger.Zero;
        }

        var gcd = BigInteger.GreatestCommonDivisor(left, right);
        return BigInteger.Abs(left / gcd * right);
    }
}
