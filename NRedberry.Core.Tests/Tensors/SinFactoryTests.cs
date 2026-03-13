using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SinFactoryTests
{
    [Fact]
    public void ShouldSimplifyKnownSinInputs()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");

        Assert.Same(argument, SinFactory.Factory.Create(new ArcSin(argument)));
        Assert.Equal(Complex.Zero, SinFactory.Factory.Create(Complex.Zero));
        Assert.IsType<Sin>(SinFactory.Factory.Create(argument));
    }
}
