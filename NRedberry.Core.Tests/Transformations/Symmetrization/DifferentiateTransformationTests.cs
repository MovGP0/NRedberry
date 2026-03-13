using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class DifferentiateTransformationTests
{
    [Fact]
    public void ShouldThrowForConstructors()
    {
        NRedberry.Tensors.SimpleTensor x = Assert.IsType<NRedberry.Tensors.SimpleTensor>(TensorApi.Parse("x"));

        Assert.Throws<NotImplementedException>(() => new DifferentiateTransformation(x));
        Assert.Throws<NotImplementedException>(() => new DifferentiateTransformation(false, x));
    }

    [Fact]
    public void ShouldThrowForStaticHelpers()
    {
        NRedberry.Tensors.SimpleTensor x = Assert.IsType<NRedberry.Tensors.SimpleTensor>(TensorApi.Parse("x"));

        Assert.Throws<NotImplementedException>(() =>
            DifferentiateTransformation.Differentiate(TensorApi.Parse("x"), x, 1));
    }
}
