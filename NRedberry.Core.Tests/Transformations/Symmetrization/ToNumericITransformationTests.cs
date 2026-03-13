using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class ToNumericITransformationTests
{
    [Fact]
    public void ShouldThrowForSingletonAccessAndHelpers()
    {
        Assert.Throws<NotImplementedException>(() => _ = ToNumericITransformation.Instance);
        Assert.Throws<NotImplementedException>(() => ToNumericITransformation.ToNumeric(TensorApi.Parse("a")));
    }
}
