using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class ToNumericITransformationTests
{
    [Fact]
    public void ShouldDelegateToToNumericTransformation()
    {
        var tensor = TensorApi.Parse("a");

        Assert.Same(ToNumericTransformation.Instance.Transform(tensor), ToNumericITransformation.Instance.Transform(tensor));
        Assert.Same(ToNumericTransformation.ToNumeric(tensor), ToNumericITransformation.ToNumeric(tensor));
    }
}
