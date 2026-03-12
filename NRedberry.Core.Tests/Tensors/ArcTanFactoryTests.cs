using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ArcTanFactoryTests
{
    [Fact]
    public void ShouldSimplifyKnownArcTanInputs()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");

        Assert.Same(argument, ArcTanFactory.Factory.Create(new Tan(argument)));
        Assert.Equal(Complex.Zero, ArcTanFactory.Factory.Create(Complex.Zero));
        Assert.IsType<ArcTan>(ArcTanFactory.Factory.Create(argument));
    }
}
