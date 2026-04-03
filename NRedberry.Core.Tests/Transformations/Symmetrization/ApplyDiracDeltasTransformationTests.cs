using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class ApplyDiracDeltasTransformationTests
{
    [Fact]
    public void ShouldLeaveTensorUntouchedWhenNoDiracDeltaIsPresent()
    {
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a+b");

        NRedberry.Tensors.Tensor actual = ApplyDiracDeltasTransformation.Instance.Transform(tensor);

        actual.ShouldBeSameAs(tensor);
    }

    [Fact]
    public void ShouldExposeReadableName()
    {
        ApplyDiracDeltasTransformation.Instance.ToString(OutputFormat.Redberry).ShouldBe("ApplyDiracDeltas");
    }
}
