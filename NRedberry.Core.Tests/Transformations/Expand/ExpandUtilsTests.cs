using NRedberry.Transformations.Fractions;
using NRedberry.Transformations.Expand;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Expand;

public sealed class ExpandUtilsTests
{
    [Fact]
    public void ShouldDetectExpandablePower()
    {
        Assert.True(ExpandUtils.IsExpandablePower(TensorApi.Parse("Power[a+b,2]")));
        Assert.False(ExpandUtils.IsExpandablePower(TensorApi.Parse("Power[a+b,x]")));
    }

    [Fact]
    public void ShouldDetectIndexedSummands()
    {
        Assert.True(ExpandUtils.SumContainsIndexed(TensorApi.Parse("A_i+B_i")));
        Assert.False(ExpandUtils.SumContainsIndexed(TensorApi.Parse("a+b")));
    }

    [Fact]
    public void ShouldApplyTransformationsSequentially()
    {
        NRedberry.Tensors.Tensor actual = ExpandUtils.Apply(
            [GetNumeratorTransformation.Instance, GetDenominatorTransformation.Instance],
            TensorApi.Parse("a/b"));

        Assert.Equal("1", actual.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldExpandPowerIntoSeparateFactors()
    {
        IList<NRedberry.Tensors.Tensor> expanded = NumeratorDenominator.ExpandPower(TensorApi.Parse("Power[a,x+y]"));

        Assert.Equal(
            ["a**x", "a**y"],
            expanded
                .Select(tensor => tensor.ToString(OutputFormat.Redberry))
                .OrderBy(term => term, StringComparer.Ordinal)
                .ToArray());
    }
}
