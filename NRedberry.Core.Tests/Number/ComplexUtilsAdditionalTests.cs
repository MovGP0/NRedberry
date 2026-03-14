using NRedberry.Numbers;
using Xunit;

namespace NRedberry.Core.Tests.Number;

public sealed class ComplexUtilsAdditionalTests
{
    private const double Delta = 1E-10;

    public static TheoryData<Action> NullGuardCalls
    {
        get
        {
            return
            [
                () => ComplexUtils.Sin(null!),
                () => ComplexUtils.Cos(null!),
                () => ComplexUtils.Tan(null!),
                () => ComplexUtils.Cot(null!),
                () => ComplexUtils.ArcSin(null!),
                () => ComplexUtils.ArcCos(null!),
                () => ComplexUtils.ArcTan(null!),
                () => ComplexUtils.ArcCot(null!),
                () => ComplexUtils.Log(null!),
                () => ComplexUtils.Exp(null!)
            ];
        }
    }

    public static TheoryData<Func<Complex, Complex>, Func<double, double>, double> RealMathCases
    {
        get
        {
            return new TheoryData<Func<Complex, Complex>, Func<double, double>, double>
            {
                { ComplexUtils.Sin, Math.Sin, 0.42 },
                { ComplexUtils.Cos, Math.Cos, 0.42 },
                { ComplexUtils.Tan, Math.Tan, 0.42 },
                { ComplexUtils.Cot, x => 1.0 / Math.Tan(x), 0.42 },
                { ComplexUtils.ArcSin, Math.Asin, 0.3 },
                { ComplexUtils.ArcCos, Math.Acos, 0.3 },
                { ComplexUtils.ArcTan, Math.Atan, 0.7 },
                { ComplexUtils.ArcCot, x => Math.Atan(1.0 / x), 0.7 },
                { ComplexUtils.Log, Math.Log, 2.5 },
                { ComplexUtils.Exp, Math.Exp, 1.2 }
            };
        }
    }

    public static TheoryData<Func<Complex, Complex>> ComplexCases
    {
        get
        {
            return
            [
                ComplexUtils.Sin,
                ComplexUtils.Cos,
                ComplexUtils.Tan,
                ComplexUtils.Cot,
                ComplexUtils.ArcSin,
                ComplexUtils.ArcCos,
                ComplexUtils.ArcTan,
                ComplexUtils.ArcCot,
                ComplexUtils.Log,
                ComplexUtils.Exp
            ];
        }
    }

    [Theory]
    [MemberData(nameof(NullGuardCalls))]
    public void MethodsShouldThrowArgumentNullExceptionOnNullInput(Action call)
    {
        Should.Throw<ArgumentNullException>(call);
    }

    [Theory]
    [MemberData(nameof(RealMathCases))]
    public void MethodsShouldMatchMathForRealInputsInValidDomains(
        Func<Complex, Complex> operation,
        Func<double, double> expectedFunction,
        double input)
    {
        Complex result = operation(new Complex(input));
        double expected = expectedFunction(input);

        result.ShouldNotBeNull();
        result.IsReal().ShouldBeTrue();
        result.Real.ToDouble().ShouldBe(expected, Delta);
    }

    [Theory]
    [MemberData(nameof(ComplexCases))]
    public void MethodsShouldReturnFiniteValueForTypicalComplexInput(Func<Complex, Complex> operation)
    {
        Complex input = new(0.5, 0.3);

        Complex result = operation(input);

        result.ShouldNotBeNull();
        result.IsNaN().ShouldBeFalse();
        result.IsInfinite().ShouldBeFalse();
    }
}
