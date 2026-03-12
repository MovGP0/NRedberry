using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class CosFactoryTests
{
    [Fact]
    public void ShouldSimplifyKnownCosInputs()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");

        Assert.Same(argument, CosFactory.Factory.Create(new ArcCos(argument)));
        Assert.Equal(Complex.One, CosFactory.Factory.Create(Complex.Zero));
        Assert.IsType<Cos>(CosFactory.Factory.Create(argument));
    }
}
