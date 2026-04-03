using NRedberry.Numbers;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class SinFactoryTests
{
    [Fact]
    public void ShouldSimplifyKnownSinInputs()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");

        SinFactory.Factory.Create(new ArcSin(argument)).ShouldBeSameAs(argument);
        SinFactory.Factory.Create(Complex.Zero).ShouldBe(Complex.Zero);
        SinFactory.Factory.Create(argument).ShouldBeOfType<Sin>();
    }
}
