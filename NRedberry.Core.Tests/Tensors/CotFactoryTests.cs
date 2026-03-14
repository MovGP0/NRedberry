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

        CotFactory.Factory.Create(new ArcCot(argument)).ShouldBeSameAs(argument);
        CotFactory.Factory.Create(Complex.Zero).ShouldBe(Complex.ComplexPositiveInfinity);
        CotFactory.Factory.Create(argument).ShouldBeOfType<Cot>();
    }
}
