using NRedberry.Transformations.Fractions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Fractions;

public sealed class NumeratorDenominatorTests
{
    [Fact]
    public void ShouldSplitSimpleFraction()
    {
        NumeratorDenominator actual = NumeratorDenominator.GetNumeratorAndDenominator(TensorApi.Parse("a/b"));

        actual.Numerator.ToString(OutputFormat.Redberry).ShouldBe("a");
        actual.Denominator.ToString(OutputFormat.Redberry).ShouldBe("b");
    }

    [Fact]
    public void ShouldUseIntegerDenominatorIndicatorForNegativePowers()
    {
        NumeratorDenominator actual = NumeratorDenominator.GetNumeratorAndDenominator(
            TensorApi.Parse("a*Power[b,-2]"),
            NumeratorDenominator.IntegerDenominatorIndicator);

        actual.Numerator.ToString(OutputFormat.Redberry).ShouldBe("a");
        actual.Denominator.ToString(OutputFormat.Redberry).ShouldBe("b**2");
    }

    [Fact]
    public void ShouldExpandPowerExponentSumIntoFactors()
    {
        IList<NRedberry.Tensors.Tensor> actual = NumeratorDenominator.ExpandPower(TensorApi.Parse("Power[a,x+y]"));

        actual
                .Select(tensor => tensor.ToString(OutputFormat.Redberry))
                .OrderBy(term => term, StringComparer.Ordinal)
                .ToArray().ShouldBe(["a**x", "a**y"]);
    }
}
