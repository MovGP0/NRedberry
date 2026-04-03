using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class IdentityTransformationTests
{
    [Fact]
    public void ShouldReturnSameTensorReference()
    {
        IdentityTransformation transformation = new();
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a+b");

        transformation.Transform(tensor).ShouldBeSameAs(tensor);
    }
}
