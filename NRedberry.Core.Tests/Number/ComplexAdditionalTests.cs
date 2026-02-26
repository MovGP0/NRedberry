using Complex32 = System.Numerics.Complex;
using NumberComplex = NRedberry.Numbers.Complex;
using Xunit;

namespace NRedberry.Core.Tests.Number;

public sealed class ComplexAdditionalTests
{
    [Fact]
    public void ShouldExposeBasicStaticConstants()
    {
        Assert.True(NumberComplex.Zero.IsZero());
        Assert.False(NumberComplex.Zero.IsOne());
        Assert.True(NumberComplex.Zero.IsReal());

        Assert.True(NumberComplex.One.IsOne());
        Assert.False(NumberComplex.One.IsZero());
        Assert.True(NumberComplex.One.IsReal());

        Assert.True(NumberComplex.ImaginaryOne.IsImaginary());
        Assert.False(NumberComplex.ImaginaryOne.IsReal());
        Assert.False(NumberComplex.ImaginaryOne.IsOne());
        Assert.False(NumberComplex.ImaginaryOne.IsZero());
    }

    [Fact]
    public void ShouldConstructFromInt()
    {
        var value = new NumberComplex(7);

        Assert.Equal(new NumberComplex(7, 0), value);
        Assert.True(value.IsReal());
        Assert.False(value.IsImaginary());
        Assert.False(value.IsNumeric());
    }

    [Fact]
    public void ShouldConstructFromDouble()
    {
        var value = new NumberComplex(2.5);

        Assert.Equal(2.5, value.Real.ToDouble(), 12);
        Assert.Equal(0.0, value.Imaginary.ToDouble(), 12);
        Assert.True(value.IsReal());
        Assert.True(value.IsNumeric());
    }

    [Fact]
    public void ShouldConstructFromComplex32()
    {
        var source = new Complex32(1.25f, -3.5f);
        var value = new NumberComplex(source);

        Assert.Equal(source.Real, value.Real.ToDouble(), 6);
        Assert.Equal(source.Imaginary, value.Imaginary.ToDouble(), 6);
        Assert.True(value.IsNumeric());
    }

    [Fact]
    public void ShouldEvaluateBasicPredicates()
    {
        var real = new NumberComplex(5, 0);
        var imaginary = new NumberComplex(0, -2);
        var one = new NumberComplex(1, 0);
        var zero = new NumberComplex(0, 0);

        Assert.True(real.IsReal());
        Assert.False(real.IsImaginary());

        Assert.True(imaginary.IsImaginary());
        Assert.False(imaginary.IsReal());

        Assert.True(one.IsOne());
        Assert.False(one.IsZero());

        Assert.True(zero.IsZero());
        Assert.False(zero.IsOne());
    }

    [Fact]
    public void ShouldConjugateComplexNumber()
    {
        var value = new NumberComplex(2, -3);
        var conjugate = value.Conjugate();

        Assert.Equal(new NumberComplex(2, 3), conjugate);
        Assert.Equal(value.Real, conjugate.Real);
        Assert.Equal(value.Imaginary.Negate(), conjugate.Imaginary);
    }

    [Fact]
    public void ShouldKeepSameInstanceWhenConjugatingRealNumber()
    {
        var real = new NumberComplex(42, 0);
        var conjugate = real.Conjugate();

        Assert.Same(real, conjugate);
    }

    [Fact]
    public void ShouldPerformBasicArithmeticOnSimpleValues()
    {
        var left = new NumberComplex(1, 2);
        var right = new NumberComplex(3, 4);

        Assert.Equal(new NumberComplex(4, 6), left.Add(right));
        Assert.Equal(new NumberComplex(-2, -2), left.Subtract(right));
        Assert.Equal(new NumberComplex(-5, 10), left.Multiply(right));

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

        Assert.Equal(NumberComplex.One, value.Pow(0));
        Assert.Equal(new NumberComplex(0, 2), value.Pow(2));
        Assert.Equal(new NumberComplex(-2, 2), value.Pow(3));

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

        Assert.Equal(first, second);
        Assert.True(first.Equals((object)second));
        Assert.Equal(first.GetHashCode(), second.GetHashCode());
        Assert.False(first.Equals(different));
    }
}
