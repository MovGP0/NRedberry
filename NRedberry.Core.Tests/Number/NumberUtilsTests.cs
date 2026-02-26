using NRedberry.Apache.Commons.Math;
using NRedberry.Numbers;
using BigInteger = System.Numerics.BigInteger;
using NumberComplex = NRedberry.Numbers.Complex;
using TensorFactory = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Number;

public sealed class NumberUtilsTests
{
    [Fact]
    public void CreateNumericShouldReturnKnownSingletonsForSpecialValues()
    {
        Assert.Same(Numeric.Zero, NumberUtils.CreateNumeric(0.0));
        Assert.Same(Numeric.One, NumberUtils.CreateNumeric(1.0));
        Assert.Same(Numeric.PositiveInfinity, NumberUtils.CreateNumeric(double.PositiveInfinity));
        Assert.Same(Numeric.NegativeInfinity, NumberUtils.CreateNumeric(double.NegativeInfinity));
        Assert.Same(Numeric.NaN, NumberUtils.CreateNumeric(double.NaN));
    }

    [Fact]
    public void CreateNumericShouldCreateNumericForRegularValues()
    {
        Numeric numeric = NumberUtils.CreateNumeric(2.5);

        Assert.Equal(2.5, numeric.DoubleValue(), 12);
    }

    [Fact]
    public void CreateRationalShouldThrowWhenFractionIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => NumberUtils.CreateRational(null!));
    }

    [Fact]
    public void CreateRationalShouldReturnKnownSingletonsForZeroAndOne()
    {
        Rational zero = NumberUtils.CreateRational(new BigFraction(0, 1));
        Rational one = NumberUtils.CreateRational(new BigFraction(1, 1));

        Assert.Same(Rational.Zero, zero);
        Assert.Same(Rational.One, one);
    }

    [Fact]
    public void CreateRationalShouldCreateRationalFromFraction()
    {
        Rational rational = NumberUtils.CreateRational(new BigFraction(3, 4));

        Assert.Equal(new Rational(3, 4), rational);
    }

    [Fact]
    public void SqrtShouldThrowForNegativeValues()
    {
        Assert.Throws<ArithmeticException>(() => NumberUtils.Sqrt(new BigInteger(-1)));
    }

    [Fact]
    public void SqrtShouldReturnZeroForZero()
    {
        BigInteger result = NumberUtils.Sqrt(BigInteger.Zero);

        Assert.Equal(BigInteger.Zero, result);
    }

    [Fact]
    public void SqrtShouldReturnExactRootForPerfectSquare()
    {
        BigInteger result = NumberUtils.Sqrt(new BigInteger(144));

        Assert.Equal(new BigInteger(12), result);
        Assert.True(NumberUtils.IsSqrt(new BigInteger(144), result));
    }

    [Fact]
    public void SqrtShouldReturnFloorForNonPerfectSquare()
    {
        BigInteger result = NumberUtils.Sqrt(new BigInteger(15));

        Assert.Equal(new BigInteger(3), result);
        Assert.False(NumberUtils.IsSqrt(new BigInteger(15), result));
    }

    [Fact]
    public void IsIntegerOddAndEvenShouldHandleIntegerAndNonIntegerValues()
    {
        Assert.True(NumberUtils.IsIntegerOdd(new NumberComplex(3, 0)));
        Assert.False(NumberUtils.IsIntegerEven(new NumberComplex(3, 0)));

        Assert.True(NumberUtils.IsIntegerEven(new NumberComplex(8, 0)));
        Assert.False(NumberUtils.IsIntegerOdd(new NumberComplex(8, 0)));

        Assert.False(NumberUtils.IsIntegerOdd(new NumberComplex(3, 1)));
        Assert.False(NumberUtils.IsIntegerEven(new NumberComplex(3, 1)));
    }

    [Fact]
    public void IsIntegerOddAndEvenShouldThrowWhenComplexIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => NumberUtils.IsIntegerOdd(null!));
        Assert.Throws<ArgumentNullException>(() => NumberUtils.IsIntegerEven(null!));
    }

    [Fact]
    public void IsZeroOrIndeterminateAndIsIndeterminateShouldClassifySpecialValues()
    {
        Assert.True(NumberUtils.IsZeroOrIndeterminate(NumberComplex.Zero));
        Assert.False(NumberUtils.IsIndeterminate(NumberComplex.Zero));

        Assert.True(NumberUtils.IsZeroOrIndeterminate(NumberComplex.RealPositiveInfinity));
        Assert.True(NumberUtils.IsIndeterminate(NumberComplex.RealPositiveInfinity));

        Assert.True(NumberUtils.IsZeroOrIndeterminate(NumberComplex.ComplexNaN));
        Assert.True(NumberUtils.IsIndeterminate(NumberComplex.ComplexNaN));

        Assert.False(NumberUtils.IsZeroOrIndeterminate(new NumberComplex(1, 0)));
        Assert.False(NumberUtils.IsIndeterminate(new NumberComplex(1, 0)));
    }

    [Fact]
    public void IndeterminateChecksShouldThrowWhenComplexIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => NumberUtils.IsZeroOrIndeterminate(null!));
        Assert.Throws<ArgumentNullException>(() => NumberUtils.IsIndeterminate(null!));
    }

    [Fact]
    public void IsRealNegativeShouldReturnTrueOnlyForNegativeRealNumbers()
    {
        Assert.True(NumberUtils.IsRealNegative(new NumberComplex(-5, 0)));
        Assert.False(NumberUtils.IsRealNegative(new NumberComplex(5, 0)));
        Assert.False(NumberUtils.IsRealNegative(new NumberComplex(-5, 1)));
    }

    [Fact]
    public void IsRealNegativeShouldThrowWhenComplexIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => NumberUtils.IsRealNegative(null!));
    }

    [Fact]
    public void IsRealNumericalShouldReturnTrueForRealNumericalExpressions()
    {
        var tensor = TensorFactory.Sum(new NumberComplex(2, 0), new NumberComplex(3, 0));

        Assert.True(NumberUtils.IsRealNumerical(tensor));
    }

    [Fact]
    public void IsRealNumericalShouldThrowWhenTensorIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => NumberUtils.IsRealNumerical(null!));
    }

    [Fact]
    public void PowShouldComputeIntegerPowers()
    {
        BigInteger bigIntegerPow = NumberUtils.Pow(new BigInteger(2), new BigInteger(10));
        long longPow = NumberUtils.Pow(3, 4);

        Assert.Equal(new BigInteger(1024), bigIntegerPow);
        Assert.Equal(81, longPow);
    }

    [Fact]
    public void PowShouldReturnOneForZeroExponent()
    {
        Assert.Equal(BigInteger.One, NumberUtils.Pow(new BigInteger(7), BigInteger.Zero));
        Assert.Equal(1L, NumberUtils.Pow(7, 0));
    }

    [Fact]
    public void FactorialShouldComputeKnownValues()
    {
        Assert.Equal(BigInteger.One, NumberUtils.Factorial(0));
        Assert.Equal(new BigInteger(120), NumberUtils.Factorial(5));
    }
}
