using NumberComplex = NRedberry.Numbers.Complex;

namespace NRedberry.Core.Tests.Number.Parser;

public sealed class ComplexTest
{
    [Fact]
    public void ShouldReturnNaNWhenDividingByZero()
    {
        NumberComplex result = NumberComplex.One.Divide(NumberComplex.Zero);

        result.IsNaN().ShouldBeTrue();
    }

    [Fact]
    public void ShouldCreateTensorComplexFromSystemComplex()
    {
        NumberComplex result = new(new System.Numerics.Complex(2.5, -1.5));

        result.ShouldBe(new NumberComplex(2.5, -1.5));
    }
}
