using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class DifferentiateTransformationTests
{
    [Fact]
    public void ShouldThrowForConstructors()
    {
        NRedberry.Tensors.SimpleTensor x = TensorApi.Parse("x").ShouldBeOfType<NRedberry.Tensors.SimpleTensor>();

        Should.Throw<NotImplementedException>(() => new DifferentiateTransformation(x));
        Should.Throw<NotImplementedException>(() => new DifferentiateTransformation(false, x));
    }

    [Fact]
    public void ShouldThrowForStaticHelpers()
    {
        NRedberry.Tensors.SimpleTensor x = TensorApi.Parse("x").ShouldBeOfType<NRedberry.Tensors.SimpleTensor>();

        Should.Throw<NotImplementedException>(() =>
            DifferentiateTransformation.Differentiate(TensorApi.Parse("x"), x, 1));
    }
}
