using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ArcSinFactoryTests
{
    [Fact]
    public void ShouldSimplifyKnownArcSinInputs()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");

        Assert.Same(argument, ArcSinFactory.Factory.Create(new Sin(argument)));
        Assert.Equal(Complex.Zero, ArcSinFactory.Factory.Create(Complex.Zero));
        Assert.IsType<ArcSin>(ArcSinFactory.Factory.Create(argument));
    }
}
