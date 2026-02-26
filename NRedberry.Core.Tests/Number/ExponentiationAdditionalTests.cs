using NRedberry.Numbers;
using Xunit;
using BigInteger = System.Numerics.BigInteger;
using TensorComplex = NRedberry.Numbers.Complex;

namespace NRedberry.Core.Tests.Number;

public sealed class ExponentiationAdditionalTests
{
    [Fact]
    public void RealExponentiateIfPossible_ShouldReturnNaN_WhenBaseIsZeroAndPowerIsInfinite()
    {
        var result = Exponentiation.ExponentiateIfPossible(Rational.Zero, Numeric.PositiveInfinity);

        Assert.NotNull(result);
        Assert.True(result.IsNaN());
    }

    [Fact]
    public void RealExponentiateIfPossible_ShouldReturnZero_WhenBaseIsZeroAndPowerIsFinite()
    {
        var result = Exponentiation.ExponentiateIfPossible(Rational.Zero, Rational.MinusOne);

        Assert.Equal(Rational.Zero, result);
    }

    [Fact]
    public void RealExponentiateIfPossible_ShouldUseIntegerPowerBranch()
    {
        var result = Exponentiation.ExponentiateIfPossible(new Rational(2, 3), new Rational(3));

        Assert.Equal(new Rational(8, 27), result);
    }

    [Fact]
    public void RealExponentiateIfPossible_ShouldReturnReducedRational_WhenPerfectRootExists()
    {
        var result = Exponentiation.ExponentiateIfPossible(new Rational(16, 81), new Rational(1, 4));

        Assert.Equal(new Rational(2, 3), result);
    }

    [Fact]
    public void RealExponentiateIfPossible_ShouldReturnNull_WhenRationalRootDoesNotExist()
    {
        var result = Exponentiation.ExponentiateIfPossible(new Rational(2, 3), new Rational(1, 2));

        Assert.Null(result);
    }

    [Fact]
    public void RealExponentiateIfPossible_ShouldHandleNegativeBaseOddAndEvenRoots()
    {
        var oddRoot = Exponentiation.ExponentiateIfPossible(new Rational(-8), new Rational(1, 3));
        var evenRoot = Exponentiation.ExponentiateIfPossible(new Rational(-16), new Rational(1, 2));

        Assert.Equal(new Rational(-2), oddRoot);
        Assert.Null(evenRoot);
    }

    [Fact]
    public void RealExponentiateIfPossible_ShouldHandleLargePerfectRootsViaBigInteger()
    {
        var hugePower = BigInteger.Pow(new BigInteger(2), 60);
        var result = Exponentiation.ExponentiateIfPossible(new Rational(hugePower), new Rational(1, 6));

        Assert.Equal(new Rational(1024), result);
    }

    [Fact]
    public void ComplexExponentiateIfPossible_ShouldHandleBaseOneAndPowerInfinity()
    {
        var result = Exponentiation.ExponentiateIfPossible(TensorComplex.One, TensorComplex.RealPositiveInfinity);

        Assert.Equal(TensorComplex.RealPositiveInfinity, result);
    }

    [Fact]
    public void ComplexExponentiateIfPossible_ShouldHandlePowerOneAndPowerZero()
    {
        var baseValue = new TensorComplex(2, 3);

        var powerOne = Exponentiation.ExponentiateIfPossible(baseValue, TensorComplex.One);
        var powerZero = Exponentiation.ExponentiateIfPossible(baseValue, TensorComplex.Zero);

        Assert.Equal(baseValue, powerOne);
        Assert.Equal(TensorComplex.One, powerZero);
    }

    [Fact]
    public void ComplexExponentiateIfPossible_ShouldHandleBaseZeroWithPositiveAndNonPositivePowers()
    {
        var positivePower = Exponentiation.ExponentiateIfPossible(TensorComplex.Zero, new TensorComplex(2));
        var zeroPower = Exponentiation.ExponentiateIfPossible(TensorComplex.Zero, TensorComplex.Zero);
        var negativePower = Exponentiation.ExponentiateIfPossible(TensorComplex.Zero, new TensorComplex(-1));

        Assert.Equal(TensorComplex.Zero, positivePower);
        Assert.Equal(TensorComplex.ComplexNaN, zeroPower);
        Assert.Equal(TensorComplex.ComplexNaN, negativePower);
    }

    [Fact]
    public void ComplexExponentiateIfPossible_ShouldUseRealIntegerPowerBranch()
    {
        var result = Exponentiation.ExponentiateIfPossible(new TensorComplex(1, 1), new TensorComplex(2));

        Assert.Equal(new TensorComplex(0, 2), result);
    }

    [Fact]
    public void ComplexExponentiateIfPossible_ShouldUseNumericBranch()
    {
        var result = Exponentiation.ExponentiateIfPossible(new TensorComplex(4.0), new TensorComplex(0.5));

        Assert.NotNull(result);
        Assert.True(result.Real.IsNumeric());
        Assert.Equal(2.0, result.Real.DoubleValue(), 10);
        Assert.Equal(0.0, result.Imaginary.DoubleValue(), 10);
    }
}
