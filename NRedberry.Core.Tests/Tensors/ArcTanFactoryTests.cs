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

        ArcTanFactory.Factory.Create(new Tan(argument)).ShouldBeSameAs(argument);
        ArcTanFactory.Factory.Create(Complex.Zero).ShouldBe(Complex.Zero);
        ArcTanFactory.Factory.Create(argument).ShouldBeOfType<ArcTan>();
    }
}
