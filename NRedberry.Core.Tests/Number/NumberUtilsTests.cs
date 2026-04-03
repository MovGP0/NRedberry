using NRedberry.Apache.Commons.Math;
using NRedberry.Numbers;
using BigInteger = System.Numerics.BigInteger;
using NumberComplex = NRedberry.Numbers.Complex;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Number;

public sealed class NumberUtilsTests
{
    [Fact]
    public void CreateNumericShouldReturnKnownSingletonsForSpecialValues()
    {
        NumberUtils.CreateNumeric(0.0).ShouldBeSameAs(Numeric.Zero);
        NumberUtils.CreateNumeric(1.0).ShouldBeSameAs(Numeric.One);
        NumberUtils.CreateNumeric(double.PositiveInfinity).ShouldBeSameAs(Numeric.PositiveInfinity);
        NumberUtils.CreateNumeric(double.NegativeInfinity).ShouldBeSameAs(Numeric.NegativeInfinity);
        NumberUtils.CreateNumeric(double.NaN).ShouldBeSameAs(Numeric.NaN);
    }

    [Fact]
    public void CreateNumericShouldCreateNumericForRegularValues()
    {
        Numeric numeric = NumberUtils.CreateNumeric(2.5);

        numeric.DoubleValue().ShouldBe(2.5, 1e-12);
    }

    [Fact]
    public void CreateRationalShouldThrowWhenFractionIsNull()
    {
        Should.Throw<ArgumentNullException>(() => NumberUtils.CreateRational(null!));
    }

    [Fact]
    public void CreateRationalShouldReturnKnownSingletonsForZeroAndOne()
    {
        Rational zero = NumberUtils.CreateRational(new BigFraction(0, 1));
        Rational one = NumberUtils.CreateRational(new BigFraction(1, 1));

        zero.ShouldBeSameAs(Rational.Zero);
        one.ShouldBeSameAs(Rational.One);
    }

    [Fact]
    public void CreateRationalShouldCreateRationalFromFraction()
    {
        Rational rational = NumberUtils.CreateRational(new BigFraction(3, 4));

        rational.ShouldBe(new Rational(3, 4));
    }

    [Fact]
    public void SqrtShouldThrowForNegativeValues()
    {
        Should.Throw<ArithmeticException>(() => NumberUtils.Sqrt(new BigInteger(-1)));
    }

    [Fact]
    public void SqrtShouldReturnZeroForZero()
    {
        BigInteger result = NumberUtils.Sqrt(BigInteger.Zero);

        result.ShouldBe(BigInteger.Zero);
    }

    [Fact]
    public void SqrtShouldReturnExactRootForPerfectSquare()
    {
        BigInteger result = NumberUtils.Sqrt(new BigInteger(144));

        result.ShouldBe(new BigInteger(12));
        NumberUtils.IsSqrt(new BigInteger(144), result).ShouldBeTrue();
    }

    [Fact]
    public void SqrtShouldReturnFloorForNonPerfectSquare()
    {
        BigInteger result = NumberUtils.Sqrt(new BigInteger(15));

        result.ShouldBe(new BigInteger(3));
        NumberUtils.IsSqrt(new BigInteger(15), result).ShouldBeFalse();
    }

    [Fact]
    public void IsIntegerOddAndEvenShouldHandleIntegerAndNonIntegerValues()
    {
        NumberUtils.IsIntegerOdd(new NumberComplex(3, 0)).ShouldBeTrue();
        NumberUtils.IsIntegerEven(new NumberComplex(3, 0)).ShouldBeFalse();

        NumberUtils.IsIntegerEven(new NumberComplex(8, 0)).ShouldBeTrue();
        NumberUtils.IsIntegerOdd(new NumberComplex(8, 0)).ShouldBeFalse();

        NumberUtils.IsIntegerOdd(new NumberComplex(3, 1)).ShouldBeFalse();
        NumberUtils.IsIntegerEven(new NumberComplex(3, 1)).ShouldBeFalse();
    }

    [Fact]
    public void IsIntegerOddAndEvenShouldThrowWhenComplexIsNull()
    {
        Should.Throw<ArgumentNullException>(() => NumberUtils.IsIntegerOdd(null!));
        Should.Throw<ArgumentNullException>(() => NumberUtils.IsIntegerEven(null!));
    }

    [Fact]
    public void IsZeroOrIndeterminateAndIsIndeterminateShouldClassifySpecialValues()
    {
        NumberUtils.IsZeroOrIndeterminate(NumberComplex.Zero).ShouldBeTrue();
        NumberUtils.IsIndeterminate(NumberComplex.Zero).ShouldBeFalse();

        NumberUtils.IsZeroOrIndeterminate(NumberComplex.RealPositiveInfinity).ShouldBeTrue();
        NumberUtils.IsIndeterminate(NumberComplex.RealPositiveInfinity).ShouldBeTrue();

        NumberUtils.IsZeroOrIndeterminate(NumberComplex.ComplexNaN).ShouldBeTrue();
        NumberUtils.IsIndeterminate(NumberComplex.ComplexNaN).ShouldBeTrue();

        NumberUtils.IsZeroOrIndeterminate(new NumberComplex(1, 0)).ShouldBeFalse();
        NumberUtils.IsIndeterminate(new NumberComplex(1, 0)).ShouldBeFalse();
    }

    [Fact]
    public void IndeterminateChecksShouldThrowWhenComplexIsNull()
    {
        Should.Throw<ArgumentNullException>(() => NumberUtils.IsZeroOrIndeterminate(null!));
        Should.Throw<ArgumentNullException>(() => NumberUtils.IsIndeterminate(null!));
    }

    [Fact]
    public void IsRealNegativeShouldReturnTrueOnlyForNegativeRealNumbers()
    {
        NumberUtils.IsRealNegative(new NumberComplex(-5, 0)).ShouldBeTrue();
        NumberUtils.IsRealNegative(new NumberComplex(5, 0)).ShouldBeFalse();
        NumberUtils.IsRealNegative(new NumberComplex(-5, 1)).ShouldBeFalse();
    }

    [Fact]
    public void IsRealNegativeShouldThrowWhenComplexIsNull()
    {
        Should.Throw<ArgumentNullException>(() => NumberUtils.IsRealNegative(null!));
    }

    [Fact]
    public void IsRealNumericalShouldReturnTrueForRealNumericalExpressions()
    {
        var tensor = TensorFactory.Sum(new NumberComplex(2, 0), new NumberComplex(3, 0));

        NumberUtils.IsRealNumerical(tensor).ShouldBeTrue();
    }

    [Fact]
    public void IsRealNumericalShouldThrowWhenTensorIsNull()
    {
        Should.Throw<ArgumentNullException>(() => NumberUtils.IsRealNumerical(null!));
    }

    [Fact]
    public void PowShouldComputeIntegerPowers()
    {
        BigInteger bigIntegerPow = NumberUtils.Pow(new BigInteger(2), new BigInteger(10));
        long longPow = NumberUtils.Pow(3, 4);

        bigIntegerPow.ShouldBe(new BigInteger(1024));
        longPow.ShouldBe(81);
    }

    [Fact]
    public void PowShouldReturnOneForZeroExponent()
    {
        NumberUtils.Pow(new BigInteger(7), BigInteger.Zero).ShouldBe(BigInteger.One);
        NumberUtils.Pow(7, 0).ShouldBe(1L);
    }

    [Fact]
    public void FactorialShouldComputeKnownValues()
    {
        NumberUtils.Factorial(0).ShouldBe(BigInteger.One);
        NumberUtils.Factorial(5).ShouldBe(new BigInteger(120));
    }
}
