using System.Numerics;
using Shouldly;
using Xunit;

namespace NRedberry.Apache.Commons.Math.Tests;

public sealed class BigFractionTests
{
    [Fact(DisplayName = "Should throw when denominator is zero")]
    public void ShouldThrowWhenDenominatorIsZero()
    {
        // Arrange
        BigInteger numerator = BigInteger.One;
        BigInteger denominator = BigInteger.Zero;

        // Act
        Action action = () => new BigFraction(numerator, denominator);

        // Assert
        action.ShouldThrow<ArgumentException>();
    }

    [Fact(DisplayName = "Should add fractions")]
    public void ShouldAddFractions()
    {
        var left = new BigFraction(1, 2);
        var right = new BigFraction(1, 3);

        BigFraction result = left.Add(right);

        result.Numerator.ShouldBe(new BigInteger(5));
        result.Denominator.ShouldBe(new BigInteger(6));
    }

    [Fact(DisplayName = "Should subtract fractions")]
    public void ShouldSubtractFractions()
    {
        var left = new BigFraction(1, 2);
        var right = new BigFraction(1, 3);

        BigFraction result = left.Subtract(right);

        result.Numerator.ShouldBe(BigInteger.One);
        result.Denominator.ShouldBe(new BigInteger(6));
    }

    [Fact(DisplayName = "Should multiply fractions")]
    public void ShouldMultiplyFractions()
    {
        var left = new BigFraction(2, 3);
        var right = new BigFraction(3, 5);

        BigFraction result = left.Multiply(right);

        result.Equals(new BigFraction(2, 5)).ShouldBeTrue();
    }

    [Fact(DisplayName = "Should multiply fraction by integer")]
    public void ShouldMultiplyFractionByInteger()
    {
        var fraction = new BigFraction(2, 3);

        BigFraction result = fraction.Multiply(4);

        result.Numerator.ShouldBe(new BigInteger(8));
        result.Denominator.ShouldBe(new BigInteger(3));
    }

    [Fact(DisplayName = "Should divide fractions")]
    public void ShouldDivideFractions()
    {
        var left = new BigFraction(2, 3);
        var right = new BigFraction(4, 5);

        BigFraction result = left.Divide(right);

        result.Equals(new BigFraction(5, 6)).ShouldBeTrue();
    }

    [Fact(DisplayName = "Should throw when dividing by zero")]
    public void ShouldThrowWhenDividingByZero()
    {
        var left = new BigFraction(1, 2);
        var right = new BigFraction(0, 1);

        Action action = () => left.Divide(right);

        action.ShouldThrow<ArgumentException>();
    }

    [Fact(DisplayName = "Should negate fractions")]
    public void ShouldNegateFractions()
    {
        var fraction = new BigFraction(2, 3);

        BigFraction result = fraction.Negate();

        result.Numerator.ShouldBe(new BigInteger(-2));
        result.Denominator.ShouldBe(new BigInteger(3));
    }

    [Fact(DisplayName = "Should compute reciprocal")]
    public void ShouldComputeReciprocal()
    {
        var fraction = new BigFraction(2, 3);

        BigFraction result = fraction.Reciprocal();

        result.Numerator.ShouldBe(new BigInteger(3));
        result.Denominator.ShouldBe(new BigInteger(2));
    }

    [Fact(DisplayName = "Should throw when computing reciprocal of zero")]
    public void ShouldThrowWhenComputingReciprocalOfZero()
    {
        var fraction = new BigFraction(0, 1);

        Action action = () => fraction.Reciprocal();

        action.ShouldThrow<ArgumentException>();
    }

    [Fact(DisplayName = "Should reduce fractions")]
    public void ShouldReduceFractions()
    {
        var fraction = new BigFraction(6, 8);

        BigFraction result = fraction.Reduce();

        result.Numerator.ShouldBe(new BigInteger(3));
        result.Denominator.ShouldBe(new BigInteger(4));
    }

    [Fact(DisplayName = "Should compare fractions by reduced value")]
    public void ShouldCompareFractionsByReducedValue()
    {
        var left = new BigFraction(1, 2);
        var right = new BigFraction(2, 4);

        left.Equals(right).ShouldBeTrue();
        (left == right).ShouldBeTrue();
        (left != right).ShouldBeFalse();
    }

    [Fact(DisplayName = "Should format as numerator over denominator")]
    public void ShouldFormatAsNumeratorOverDenominator()
    {
        var fraction = new BigFraction(3, 4);

        fraction.ToString().ShouldBe("3/4");
    }
}
