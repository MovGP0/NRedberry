using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ArcCosFactoryTests
{
    [Fact]
    public void ShouldSimplifyKnownArcCosInputs()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");

        ArcCosFactory.Factory.Create(new Cos(argument)).ShouldBeSameAs(argument);
        ArcCosFactory.Factory.Create(Complex.Zero).ToString(OutputFormat.Redberry).ShouldBe(TensorApi.Parse("pi/2").ToString(OutputFormat.Redberry));
        ArcCosFactory.Factory.Create(argument).ShouldBeOfType<ArcCos>();
    }
}
