using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors;

public sealed class ArcSinFactoryTests
{
    [Fact]
    public void ShouldSimplifyKnownArcSinInputs()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");

        ArcSinFactory.Factory.Create(new Sin(argument)).ShouldBeSameAs(argument);
        ArcSinFactory.Factory.Create(Complex.Zero).ShouldBe(Complex.Zero);
        ArcSinFactory.Factory.Create(argument).ShouldBeOfType<ArcSin>();
    }
}
