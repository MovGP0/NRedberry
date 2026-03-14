using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class ITransformationTests
{
    [Fact]
    public void ShouldSupportInterfaceDispatch()
    {
        ITransformation transformation = new IdentityTransformation();
        NRedberry.Tensors.Tensor tensor = TensorApi.Parse("a");

        transformation.Transform(tensor).ShouldBeSameAs(tensor);
    }
}
