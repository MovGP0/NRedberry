using NRedberry.Transformations.Factor;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Factor;

public sealed class FactorTransformationTests
{
    [Fact]
    public void ShouldFactorExpandedPerfectSquare()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a");

        NRedberry.Tensors.Tensor actual = FactorTransformation.Factor(tensor);

        Assert.Equal(
            tensor.ToString(OutputFormat.Redberry),
            actual.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        Assert.Equal("Factor", FactorTransformation.Instance.ToString(OutputFormat.Redberry));
    }
}
