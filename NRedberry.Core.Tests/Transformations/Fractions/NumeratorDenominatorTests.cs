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

        Assert.Equal("a", actual.Numerator.ToString(OutputFormat.Redberry));
        Assert.Equal("b", actual.Denominator.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldUseIntegerDenominatorIndicatorForNegativePowers()
    {
        NumeratorDenominator actual = NumeratorDenominator.GetNumeratorAndDenominator(
            TensorApi.Parse("a*Power[b,-2]"),
            NumeratorDenominator.IntegerDenominatorIndicator);

        Assert.Equal("a", actual.Numerator.ToString(OutputFormat.Redberry));
        Assert.Equal("b**2", actual.Denominator.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldExpandPowerExponentSumIntoFactors()
    {
        IList<NRedberry.Tensors.Tensor> actual = NumeratorDenominator.ExpandPower(TensorApi.Parse("Power[a,x+y]"));

        Assert.Equal(
            ["a**x", "a**y"],
            actual
                .Select(tensor => tensor.ToString(OutputFormat.Redberry))
                .OrderBy(term => term, StringComparer.Ordinal)
                .ToArray());
    }
}
