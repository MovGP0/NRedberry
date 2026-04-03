using NRedberry.Numbers;
using NRedberry.Tensors.Functions;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors;

public sealed class LogFactoryTests
{
    [Fact]
    public void ShouldSimplifyKnownLogInputs()
    {
        NRedberry.Tensors.Tensor argument = TensorApi.Parse("a");

        LogFactory.Factory.Create(new Exp(argument)).ShouldBeSameAs(argument);
        LogFactory.Factory.Create(Complex.One).ShouldBe(Complex.Zero);
        LogFactory.Factory.Create(argument).ShouldBeOfType<Log>();
    }
}
