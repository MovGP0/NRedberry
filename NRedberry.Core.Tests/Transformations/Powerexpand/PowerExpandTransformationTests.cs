using NRedberry.Core.Utils;
using NRedberry.Transformations.Powerexpand;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Powerexpand;

public sealed class PowerExpandTransformationTests
{
    [Fact]
    public void ShouldThrowForSingletonAccess()
    {
        Assert.Throws<NotImplementedException>(() => _ = PowerExpandTransformation.Instance);
    }

    [Fact]
    public void ShouldThrowForPublicConstructors()
    {
        Assert.Throws<NotImplementedException>(() =>
            new PowerExpandTransformation(new TrueIndicator<NRedberry.Tensors.Tensor>()));
        Assert.Throws<NotImplementedException>(() =>
            new PowerExpandTransformation([Assert.IsType<NRedberry.Tensors.SimpleTensor>(TensorApi.Parse("x"))]));
    }
}
