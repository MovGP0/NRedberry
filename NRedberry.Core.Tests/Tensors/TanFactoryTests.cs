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

        TanFactory.Factory.Create(new ArcTan(argument)).ShouldBeSameAs(argument);
        TanFactory.Factory.Create(Complex.Zero).ShouldBe(Complex.Zero);
        TanFactory.Factory.Create(argument).ShouldBeOfType<Tan>();
    }
}
