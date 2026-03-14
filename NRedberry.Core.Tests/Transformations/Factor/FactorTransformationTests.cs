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

        actual.ToString(OutputFormat.Redberry).ShouldBe(tensor.ToString(OutputFormat.Redberry));
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        FactorTransformation.Instance.ToString(OutputFormat.Redberry).ShouldBe("Factor");
    }
}
