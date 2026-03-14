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

        result.ShouldNotBeNull();
        result.IsNaN().ShouldBeTrue();
    }

    [Fact]
    public void RealExponentiateIfPossible_ShouldReturnZero_WhenBaseIsZeroAndPowerIsFinite()
    {
        var result = Exponentiation.ExponentiateIfPossible(Rational.Zero, Rational.MinusOne);

        result.ShouldBe(Rational.Zero);
    }

    [Fact]
    public void RealExponentiateIfPossible_ShouldUseIntegerPowerBranch()
    {
        var result = Exponentiation.ExponentiateIfPossible(new Rational(2, 3), new Rational(3));

        result.ShouldBe(new Rational(8, 27));
    }

    [Fact]
    public void RealExponentiateIfPossible_ShouldReturnReducedRational_WhenPerfectRootExists()
    {
        var result = Exponentiation.ExponentiateIfPossible(new Rational(16, 81), new Rational(1, 4));

        result.ShouldBe(new Rational(2, 3));
    }

    [Fact]
    public void RealExponentiateIfPossible_ShouldReturnNull_WhenRationalRootDoesNotExist()
    {
        var result = Exponentiation.ExponentiateIfPossible(new Rational(2, 3), new Rational(1, 2));

        result.ShouldBeNull();
    }

    [Fact]
    public void RealExponentiateIfPossible_ShouldHandleNegativeBaseOddAndEvenRoots()
    {
        var oddRoot = Exponentiation.ExponentiateIfPossible(new Rational(-8), new Rational(1, 3));
        var evenRoot = Exponentiation.ExponentiateIfPossible(new Rational(-16), new Rational(1, 2));

        oddRoot.ShouldBe(new Rational(-2));
        evenRoot.ShouldBeNull();
    }

    [Fact]
    public void RealExponentiateIfPossible_ShouldHandleLargePerfectRootsViaBigInteger()
    {
        var hugePower = BigInteger.Pow(new BigInteger(2), 60);
        var result = Exponentiation.ExponentiateIfPossible(new Rational(hugePower), new Rational(1, 6));

        result.ShouldBe(new Rational(1024));
    }

    [Fact]
    public void ComplexExponentiateIfPossible_ShouldHandleBaseOneAndPowerInfinity()
    {
        var result = Exponentiation.ExponentiateIfPossible(TensorComplex.One, TensorComplex.RealPositiveInfinity);

        result.ShouldBe(TensorComplex.RealPositiveInfinity);
    }

    [Fact]
    public void ComplexExponentiateIfPossible_ShouldHandlePowerOneAndPowerZero()
    {
        var baseValue = new TensorComplex(2, 3);

        var powerOne = Exponentiation.ExponentiateIfPossible(baseValue, TensorComplex.One);
        var powerZero = Exponentiation.ExponentiateIfPossible(baseValue, TensorComplex.Zero);

        powerOne.ShouldBe(baseValue);
        powerZero.ShouldBe(TensorComplex.One);
    }

    [Fact]
    public void ComplexExponentiateIfPossible_ShouldHandleBaseZeroWithPositiveAndNonPositivePowers()
    {
        var positivePower = Exponentiation.ExponentiateIfPossible(TensorComplex.Zero, new TensorComplex(2));
        var zeroPower = Exponentiation.ExponentiateIfPossible(TensorComplex.Zero, TensorComplex.Zero);
        var negativePower = Exponentiation.ExponentiateIfPossible(TensorComplex.Zero, new TensorComplex(-1));

        positivePower.ShouldBe(TensorComplex.Zero);
        zeroPower.ShouldBe(TensorComplex.ComplexNaN);
        negativePower.ShouldBe(TensorComplex.ComplexNaN);
    }

    [Fact]
    public void ComplexExponentiateIfPossible_ShouldUseRealIntegerPowerBranch()
    {
        var result = Exponentiation.ExponentiateIfPossible(new TensorComplex(1, 1), new TensorComplex(2));

        result.ShouldBe(new TensorComplex(0, 2));
    }

    [Fact]
    public void ComplexExponentiateIfPossible_ShouldUseNumericBranch()
    {
        var result = Exponentiation.ExponentiateIfPossible(new TensorComplex(4.0), new TensorComplex(0.5));

        result.ShouldNotBeNull();
        result.Real.IsNumeric().ShouldBeTrue();
        result.Real.DoubleValue().ShouldBe(2.0, 1e-10);
        result.Imaginary.DoubleValue().ShouldBe(0.0, 1e-10);
    }
}
