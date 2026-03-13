using NRedberry.Transformations.Fractions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Fractions;

public sealed class GetDenominatorTransformationTests
{
    [Fact]
    public void ShouldExtractDenominator()
    {
        NRedberry.Tensors.Tensor actual = GetDenominatorTransformation.Instance.Transform(TensorApi.Parse("a/b"));

        Assert.Equal("b", actual.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        Assert.Equal("Denominator", GetDenominatorTransformation.Instance.ToString(OutputFormat.Redberry));
    }
}
