using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class TanFactoryTests
{
    [Fact]
    public void ShouldSimplifyKnownTanInputs()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");

        Assert.Same(argument, TanFactory.Factory.Create(new ArcTan(argument)));
        Assert.Equal(Complex.Zero, TanFactory.Factory.Create(Complex.Zero));
        Assert.IsType<Tan>(TanFactory.Factory.Create(argument));
    }
}
