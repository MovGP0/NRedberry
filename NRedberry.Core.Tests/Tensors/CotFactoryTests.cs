using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class CotFactoryTests
{
    [Fact]
    public void ShouldSimplifyKnownCotInputs()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");

        Assert.Same(argument, CotFactory.Factory.Create(new ArcCot(argument)));
        Assert.Equal(Complex.ComplexPositiveInfinity, CotFactory.Factory.Create(Complex.Zero));
        Assert.IsType<Cot>(CotFactory.Factory.Create(argument));
    }
}
