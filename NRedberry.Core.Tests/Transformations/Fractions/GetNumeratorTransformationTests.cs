using NRedberry.Transformations.Fractions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Fractions;

public sealed class GetNumeratorTransformationTests
{
    [Fact]
    public void ShouldExtractNumerator()
    {
        NRedberry.Tensors.Tensor actual = GetNumeratorTransformation.Instance.Transform(TensorApi.Parse("a/b"));

        Assert.Equal("a", actual.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        Assert.Equal("Numerator", GetNumeratorTransformation.Instance.ToString(OutputFormat.Redberry));
    }
}
