using System.Numerics;
using NRedberry.Numbers;
using Xunit;
using NumberComplex = NRedberry.Numbers.Complex;

namespace NRedberry.Core.Tests.Number.Parser;

public sealed class ComplexTest
{
    [Fact]
    public void ShouldReturnNaNWhenDividingByZero()
    {
        NumberComplex result = NumberComplex.One.Divide(NumberComplex.Zero);

        Assert.True(result.IsNaN());
    }

    [Fact]
    public void ShouldCreateTensorComplexFromSystemComplex()
    {
        NumberComplex result = new(new System.Numerics.Complex(2.5, -1.5));

        Assert.Equal(new NumberComplex(2.5, -1.5), result);
    }
}
