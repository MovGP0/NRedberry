using Complex32 = System.Numerics.Complex;
using NumberComplex = NRedberry.Numbers.Complex;
using Xunit;

namespace NRedberry.Core.Tests.Number;

public sealed class ComplexAdditionalTests
{
    [Fact]
    public void ShouldExposeBasicStaticConstants()
    {
        NumberComplex.Zero.IsZero().ShouldBeTrue();
        NumberComplex.Zero.IsOne().ShouldBeFalse();
        NumberComplex.Zero.IsReal().ShouldBeTrue();

        NumberComplex.One.IsOne().ShouldBeTrue();
        NumberComplex.One.IsZero().ShouldBeFalse();
        NumberComplex.One.IsReal().ShouldBeTrue();

        NumberComplex.ImaginaryOne.IsImaginary().ShouldBeTrue();
        NumberComplex.ImaginaryOne.IsReal().ShouldBeFalse();
        NumberComplex.ImaginaryOne.IsOne().ShouldBeFalse();
        NumberComplex.ImaginaryOne.IsZero().ShouldBeFalse();
    }

    [Fact]
    public void ShouldConstructFromInt()
    {
        var value = new NumberComplex(7);

        value.ShouldBe(new NumberComplex(7, 0));
        value.IsReal().ShouldBeTrue();
        value.IsImaginary().ShouldBeFalse();
        value.IsNumeric().ShouldBeFalse();
    }

    [Fact]
    public void ShouldConstructFromDouble()
    {
        var value = new NumberComplex(2.5);

        Assert.Equal(2.5, value.Real.ToDouble(), 12);
        Assert.Equal(0.0, value.Imaginary.ToDouble(), 12);
        value.IsReal().ShouldBeTrue();
        value.IsNumeric().ShouldBeTrue();
    }

    [Fact]
    public void ShouldConstructFromComplex32()
    {
        var source = new Complex32(1.25f, -3.5f);
        var value = new NumberComplex(source);

        Assert.Equal(source.Real, value.Real.ToDouble(), 6);
        Assert.Equal(source.Imaginary, value.Imaginary.ToDouble(), 6);
        value.IsNumeric().ShouldBeTrue();
    }

    [Fact]
    public void ShouldEvaluateBasicPredicates()
    {
        var real = new NumberComplex(5, 0);
        var imaginary = new NumberComplex(0, -2);
        var one = new NumberComplex(1, 0);
        var zero = new NumberComplex(0, 0);

        real.IsReal().ShouldBeTrue();
        real.IsImaginary().ShouldBeFalse();

        imaginary.IsImaginary().ShouldBeTrue();
        imaginary.IsReal().ShouldBeFalse();

        one.IsOne().ShouldBeTrue();
        one.IsZero().ShouldBeFalse();

        zero.IsZero().ShouldBeTrue();
        zero.IsOne().ShouldBeFalse();
    }

    [Fact]
    public void ShouldConjugateComplexNumber()
    {
        var value = new NumberComplex(2, -3);
        var conjugate = value.Conjugate();

        conjugate.ShouldBe(new NumberComplex(2, 3));
        conjugate.Real.ShouldBe(value.Real);
        conjugate.Imaginary.ShouldBe(value.Imaginary.Negate());
    }

    [Fact]
    public void ShouldKeepSameInstanceWhenConjugatingRealNumber()
    {
        var real = new NumberComplex(42, 0);
        var conjugate = real.Conjugate();

        conjugate.ShouldBeSameAs(real);
    }

    [Fact]
    public void ShouldPerformBasicArithmeticOnSimpleValues()
    {
        var left = new NumberComplex(1, 2);
        var right = new NumberComplex(3, 4);

        left.Add(right).ShouldBe(new NumberComplex(4, 6));
        left.Subtract(right).ShouldBe(new NumberComplex(-2, -2));
        left.Multiply(right).ShouldBe(new NumberComplex(-5, 10));

        var quotient = left.Divide(right);
        Assert.Equal(11.0 / 25.0, quotient.Real.ToDouble(), 12);
        Assert.Equal(2.0 / 25.0, quotient.Imaginary.ToDouble(), 12);
    }

    [Fact]
    public void ShouldComputeAbsNumericForKnownValues()
    {
        Assert.Equal(5.0, new NumberComplex(3, 4).AbsNumeric(), 12);
        Assert.Equal(7.0, new NumberComplex(0, -7).AbsNumeric(), 12);
        Assert.Equal(13.0, NumberComplex.AbsNumeric(5.0, 12.0), 12);
    }

    [Fact]
    public void ShouldRaiseToSmallIntegerPowers()
    {
        var value = new NumberComplex(1, 1);

        value.Pow(0).ShouldBe(NumberComplex.One);
        value.Pow(2).ShouldBe(new NumberComplex(0, 2));
        value.Pow(3).ShouldBe(new NumberComplex(-2, 2));

        var inverse = value.Pow(-1);
        Assert.Equal(0.5, inverse.Real.ToDouble(), 12);
        Assert.Equal(-0.5, inverse.Imaginary.ToDouble(), 12);
    }

    [Fact]
    public void ShouldProvideConsistentEqualsAndHashCode()
    {
        var first = new NumberComplex(9, -4);
        var second = new NumberComplex(9, -4);
        var different = new NumberComplex(9, 4);

        second.ShouldBe(first);
        first.Equals((object)second).ShouldBeTrue();
        second.GetHashCode().ShouldBe(first.GetHashCode());
        first.Equals(different).ShouldBeFalse();
    }
}
