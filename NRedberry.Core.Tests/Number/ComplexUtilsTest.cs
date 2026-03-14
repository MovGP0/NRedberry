using NRedberry.Numbers;
using NRedberry.Numbers.Parser;
using NumberComplex = NRedberry.Numbers.Complex;
using Xunit;

namespace NRedberry.Core.Tests.Number;

public sealed class ComplexUtilsTest
{
    private const double Delta = 1E-10;

    [Fact]
    public void ShouldRoundTripComplexFunctions()
    {
        NumberComplex realInput = new(0.3);
        NumberComplex complexInput = NumberParser<NumberComplex>.ComplexParser.Parse("1+2*I");

        (realInput.Subtract(ComplexUtils.Sin(ComplexUtils.ArcSin(realInput))).AbsNumeric() <= Delta).ShouldBeTrue();
        (realInput.Subtract(ComplexUtils.Cos(ComplexUtils.ArcCos(realInput))).AbsNumeric() <= Delta).ShouldBeTrue();
        (realInput.Subtract(ComplexUtils.Tan(ComplexUtils.ArcTan(realInput))).AbsNumeric() <= Delta).ShouldBeTrue();
        (realInput.Subtract(ComplexUtils.Cot(ComplexUtils.ArcCot(realInput))).AbsNumeric() <= Delta).ShouldBeTrue();
        (realInput.Subtract(ComplexUtils.Log(ComplexUtils.Exp(realInput))).AbsNumeric() <= Delta).ShouldBeTrue();
        (complexInput.Subtract(ComplexUtils.Exp(ComplexUtils.Log(complexInput))).AbsNumeric() <= 1E-5).ShouldBeTrue();
    }
}
