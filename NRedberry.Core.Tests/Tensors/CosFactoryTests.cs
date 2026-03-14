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

        CosFactory.Factory.Create(new ArcCos(argument)).ShouldBeSameAs(argument);
        CosFactory.Factory.Create(Complex.Zero).ShouldBe(Complex.One);
        CosFactory.Factory.Create(argument).ShouldBeOfType<Cos>();
    }
}
