using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class ToNumericITransformationTests
{
    [Fact]
    public void ShouldDelegateToToNumericTransformation()
    {
        var tensor = TensorApi.Parse("a");

        ToNumericITransformation.Instance.Transform(tensor).ShouldBeSameAs(ToNumericTransformation.Instance.Transform(tensor));
        ToNumericITransformation.ToNumeric(tensor).ShouldBeSameAs(ToNumericTransformation.ToNumeric(tensor));
    }
}
